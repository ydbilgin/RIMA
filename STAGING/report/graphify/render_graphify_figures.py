from __future__ import annotations

import json
import math
import shutil
import time
from pathlib import Path

import matplotlib

matplotlib.use("Agg")
import matplotlib.pyplot as plt
import networkx as nx
from matplotlib.lines import Line2D
from pyvis.network import Network


ROOT = Path(__file__).resolve().parents[3]
INPUT = ROOT / "STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json"
OUT_DIR = ROOT / "STAGING/report/graphify"

FULL_PNG = OUT_DIR / "full_graph.png"
FOCUS_PNG = OUT_DIR / "god_nodes_focus.png"
INTERACTIVE_HTML = OUT_DIR / "graph_interactive.html"

EDITOR = "#E89020"
RUNTIME = "#5A7AA0"
NEIGHBOR = "#B9BEC6"
EDGE = "#B7BAC1"

GOD_NODES = [
    ("DirectorMode", 168, "editor"),
    ("InPlayMapPaintOverlay", 93, "editor"),
    ("RoomPainterWindow", 88, "editor"),
    ("LargeDungeonMapPainterBase", 78, "editor"),
    ("MinimalTilePainter", 70, "editor"),
    ("BuildPlacementController", 66, "editor"),
    ("ChamberSelectBootstrap", 84, "runtime"),
    ("CharacterSelectScreen", 75, "runtime"),
    ("RuntimeRoomManager", 69, "runtime"),
    ("RoomRunDirector", 65, "runtime"),
]


def load_graph() -> nx.Graph:
    data = json.loads(INPUT.read_text(encoding="utf-8"))
    graph = nx.Graph()
    for node in data["nodes"]:
        node_id = node["id"]
        graph.add_node(node_id, **node)
    for link in data["links"]:
        src = link.get("_src")
        tgt = link.get("_tgt")
        if not src or not tgt:
            continue
        graph.add_edge(src, tgt, **link)
    return graph


def match_god_nodes(graph: nx.Graph) -> dict[str, dict[str, object]]:
    by_label = {str(attrs.get("label", "")): node for node, attrs in graph.nodes(data=True)}
    by_norm = {str(attrs.get("norm_label", "")): node for node, attrs in graph.nodes(data=True)}
    matches: dict[str, dict[str, object]] = {}
    for label, stated_degree, kind in GOD_NODES:
        node_id = by_label.get(label)
        if node_id is None:
            norm = label.lower()
            node_id = by_norm.get(norm)
        if node_id is None:
            norm = label.lower()
            for node, attrs in graph.nodes(data=True):
                raw_label = str(attrs.get("label", "")).lower()
                raw_norm = str(attrs.get("norm_label", "")).lower()
                if norm in raw_label or norm in raw_norm:
                    node_id = node
                    break
        if node_id is not None:
            matches[label] = {
                "id": node_id,
                "label": label,
                "stated_degree": stated_degree,
                "kind": kind,
                "actual_degree": graph.degree(node_id),
            }
    return matches


def god_color(kind: str) -> str:
    return EDITOR if kind == "editor" else RUNTIME


def graphviz_available() -> bool:
    return shutil.which("sfdp") is not None


def compute_full_layout(graph: nx.Graph) -> tuple[dict[str, tuple[float, float]], str, float]:
    started = time.perf_counter()
    if graphviz_available():
        try:
            from networkx.drawing.nx_pydot import graphviz_layout

            pos = graphviz_layout(graph, prog="sfdp")
            return pos, "graphviz_sfdp", time.perf_counter() - started
        except Exception:
            pass

    pos = nx.spring_layout(
        graph,
        seed=42,
        iterations=18,
        threshold=0.03,
        weight="weight",
        method="energy",
        scale=10.0,
    )
    return pos, "spring_layout_energy_iter18", time.perf_counter() - started


def render_full_graph(
    graph: nx.Graph,
    pos: dict[str, tuple[float, float]],
    matches: dict[str, dict[str, object]],
    layout_name: str,
) -> None:
    fig, ax = plt.subplots(figsize=(14, 10), dpi=300)
    ax.set_facecolor("#FAFAF8")
    ax.axis("off")

    god_ids = {str(info["id"]) for info in matches.values()}
    normal_nodes = [node for node in graph.nodes if node not in god_ids]

    nx.draw_networkx_edges(
        graph,
        pos,
        ax=ax,
        edge_color=EDGE,
        alpha=0.055,
        width=0.22,
        arrows=False,
    )
    nx.draw_networkx_nodes(
        graph,
        pos,
        nodelist=normal_nodes,
        node_size=5,
        node_color="#7C8087",
        alpha=0.19,
        linewidths=0,
        ax=ax,
    )

    for label, info in matches.items():
        node_id = str(info["id"])
        kind = str(info["kind"])
        nx.draw_networkx_nodes(
            graph,
            pos,
            nodelist=[node_id],
            node_size=370 if kind == "editor" else 330,
            node_color=god_color(kind),
            edgecolors="#222222",
            linewidths=0.75,
            alpha=0.96,
            ax=ax,
        )

    for label, info in matches.items():
        node_id = str(info["id"])
        x, y = pos[node_id]
        ax.text(
            x,
            y + 0.08,
            label,
            fontsize=6.5,
            color="#1D1F24",
            ha="center",
            va="bottom",
            bbox={"boxstyle": "round,pad=0.18", "facecolor": "#FFFFFF", "edgecolor": "none", "alpha": 0.72},
        )

    handles = [
        Line2D([0], [0], marker="o", color="w", label="editor/tooling (6)", markerfacecolor=EDITOR, markersize=8),
        Line2D([0], [0], marker="o", color="w", label="game/runtime (4)", markerfacecolor=RUNTIME, markersize=8),
    ]
    ax.legend(handles=handles, loc="lower left", frameon=False, fontsize=8)
    ax.set_title(
        "RIMA codebase - 6925 nodes, 14321 edges - 6 of 10 most-connected = editor/tooling",
        fontsize=12,
        color="#1D1F24",
        pad=14,
    )
    ax.text(0.99, 0.01, f"layout: {layout_name}", transform=ax.transAxes, ha="right", va="bottom", fontsize=6, color="#6F747B")
    fig.savefig(FULL_PNG, bbox_inches="tight", pad_inches=0.18)
    plt.close(fig)


def strongest_focus_subgraph(graph: nx.Graph, matches: dict[str, dict[str, object]], max_nodes: int = 145) -> nx.Graph:
    god_ids = {str(info["id"]) for info in matches.values()}
    nodes = set(god_ids)
    for god_id in god_ids:
        nodes.update(graph.neighbors(god_id))
    if len(nodes) <= max_nodes:
        return graph.subgraph(nodes).copy()

    neighbor_scores: dict[str, float] = {}
    for node in nodes - god_ids:
        score = graph.degree(node)
        for god_id in god_ids:
            if graph.has_edge(node, god_id):
                score += float(graph.edges[node, god_id].get("weight", 1.0)) * 10.0
        neighbor_scores[node] = score

    keep = set(god_ids)
    keep.update(node for node, _ in sorted(neighbor_scores.items(), key=lambda item: item[1], reverse=True)[: max_nodes - len(keep)])
    return graph.subgraph(keep).copy()


def render_focus_graph(graph: nx.Graph, matches: dict[str, dict[str, object]]) -> tuple[int, str]:
    focus = strongest_focus_subgraph(graph, matches)
    pos = nx.spring_layout(focus, seed=84, iterations=130, threshold=0.002, weight="weight", scale=3.0)

    god_by_id = {str(info["id"]): info for info in matches.values()}
    node_sizes = []
    node_colors = []
    node_alpha = []
    for node in focus.nodes:
        if node in god_by_id:
            kind = str(god_by_id[node]["kind"])
            node_colors.append(god_color(kind))
            node_sizes.append(290 + graph.degree(node) * 7.0)
            node_alpha.append(0.98)
        else:
            node_colors.append(NEIGHBOR)
            node_sizes.append(35 + min(graph.degree(node), 22) * 3.0)
            node_alpha.append(0.42)

    fig, ax = plt.subplots(figsize=(12, 9), dpi=300)
    ax.set_facecolor("#FAFAF8")
    ax.axis("off")
    nx.draw_networkx_edges(focus, pos, ax=ax, edge_color=EDGE, alpha=0.22, width=0.45)
    nx.draw_networkx_nodes(
        focus,
        pos,
        ax=ax,
        node_size=node_sizes,
        node_color=node_colors,
        alpha=node_alpha,
        edgecolors="#2C2D30",
        linewidths=0.35,
    )

    for label, info in matches.items():
        node_id = str(info["id"])
        if node_id not in focus:
            continue
        x, y = pos[node_id]
        ax.text(
            x,
            y + 0.055,
            f"{label}\n{info['actual_degree']}",
            fontsize=8,
            color="#111316",
            ha="center",
            va="bottom",
            linespacing=0.95,
            bbox={"boxstyle": "round,pad=0.2", "facecolor": "#FFFFFF", "edgecolor": "#D6D8DC", "alpha": 0.86},
        )

    handles = [
        Line2D([0], [0], marker="o", color="w", label="editor tools", markerfacecolor=EDITOR, markersize=8),
        Line2D([0], [0], marker="o", color="w", label="game/runtime", markerfacecolor=RUNTIME, markersize=8),
        Line2D([0], [0], marker="o", color="w", label="direct neighbors", markerfacecolor=NEIGHBOR, markersize=7),
    ]
    ax.legend(handles=handles, loc="lower left", frameon=False, fontsize=8)
    ax.set_title(
        "10 most-connected nodes - 6 are editor tools (orange), 4 are game/runtime (blue)",
        fontsize=12,
        color="#1D1F24",
        pad=14,
    )
    fig.savefig(FOCUS_PNG, bbox_inches="tight", pad_inches=0.18)
    plt.close(fig)
    return focus.number_of_nodes(), "spring_layout_iter130"


def interactive_subgraph(graph: nx.Graph, matches: dict[str, dict[str, object]], max_nodes: int = 650) -> nx.Graph:
    god_ids = {str(info["id"]) for info in matches.values()}
    one_hop = set(god_ids)
    two_hop = set()
    for god_id in god_ids:
        neighbors = set(graph.neighbors(god_id))
        one_hop.update(neighbors)
        for neighbor in neighbors:
            two_hop.update(graph.neighbors(neighbor))

    nodes = one_hop | two_hop
    if len(nodes) <= max_nodes:
        return graph.subgraph(nodes).copy()

    scores: dict[str, float] = {}
    for node in nodes - god_ids:
        score = graph.degree(node)
        if node in one_hop:
            score += 1000.0
        for god_id in god_ids:
            if graph.has_edge(node, god_id):
                score += 100.0
        scores[node] = score
    keep = set(god_ids)
    keep.update(node for node, _ in sorted(scores.items(), key=lambda item: item[1], reverse=True)[: max_nodes - len(keep)])
    return graph.subgraph(keep).copy()


def render_interactive(graph: nx.Graph, matches: dict[str, dict[str, object]]) -> int:
    subgraph = interactive_subgraph(graph, matches)
    god_by_id = {str(info["id"]): info for info in matches.values()}
    net = Network(
        height="100vh",
        width="100%",
        bgcolor="#FAFAF8",
        font_color="#1D1F24",
        cdn_resources="in_line",
        directed=False,
    )
    net.barnes_hut(gravity=-18000, central_gravity=0.2, spring_length=120, spring_strength=0.025, damping=0.75, overlap=0.2)

    for node, attrs in subgraph.nodes(data=True):
        label = str(attrs.get("label", node))
        degree = graph.degree(node)
        if node in god_by_id:
            info = god_by_id[node]
            color = god_color(str(info["kind"]))
            size = 28 + math.sqrt(degree) * 3.0
            shown_label = label
            border = "#202226"
        else:
            color = "rgba(150,154,164,0.42)"
            size = 5 + min(degree, 30) * 0.35
            shown_label = ""
            border = "rgba(150,154,164,0.35)"
        title = (
            f"<b>{label}</b><br>"
            f"degree: {degree}<br>"
            f"file_type: {attrs.get('file_type', '')}<br>"
            f"source: {attrs.get('source_file', '')}"
        )
        net.add_node(
            node,
            label=shown_label,
            title=title,
            color={"background": color, "border": border},
            size=size,
            font={"size": 20 if node in god_by_id else 9, "face": "Arial"},
        )

    for src, tgt, attrs in subgraph.edges(data=True):
        weight = float(attrs.get("weight", 1.0))
        net.add_edge(src, tgt, value=max(weight, 0.2), color="rgba(125,130,140,0.18)", width=0.35)

    net.set_options(
        """
        {
          "interaction": {"hover": true, "tooltipDelay": 80, "hideEdgesOnDrag": true},
          "physics": {
            "enabled": true,
            "stabilization": {"enabled": true, "iterations": 220, "updateInterval": 25},
            "minVelocity": 0.75
          },
          "nodes": {"shape": "dot"},
          "edges": {"smooth": false}
        }
        """
    )
    html = net.generate_html(notebook=False)
    INTERACTIVE_HTML.write_text(html, encoding="utf-8")
    return subgraph.number_of_nodes()


def file_kb(path: Path) -> float:
    return path.stat().st_size / 1024.0


def main() -> None:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    graph = load_graph()
    matches = match_god_nodes(graph)
    if len(matches) != len(GOD_NODES):
        missing = sorted({label for label, _, _ in GOD_NODES} - set(matches))
        raise RuntimeError(f"Missing god-node matches: {missing}")

    pos, full_layout, full_seconds = compute_full_layout(graph)
    render_full_graph(graph, pos, matches, full_layout)
    focus_nodes, focus_layout = render_focus_graph(graph, matches)
    interactive_nodes = render_interactive(graph, matches)

    outputs = [FULL_PNG, FOCUS_PNG, INTERACTIVE_HTML]
    for output in outputs:
        if not output.exists():
            raise RuntimeError(f"Missing output: {output}")
    for output in [FULL_PNG, FOCUS_PNG]:
        if output.stat().st_size <= 100 * 1024:
            raise RuntimeError(f"Output unexpectedly small: {output} ({file_kb(output):.1f} KB)")
    if "<html" not in INTERACTIVE_HTML.read_text(encoding="utf-8", errors="ignore").lower():
        raise RuntimeError(f"HTML did not render as a document: {INTERACTIVE_HTML}")

    print(f"full_graph.png | {graph.number_of_nodes()} nodes/{graph.number_of_edges()} edges | {file_kb(FULL_PNG):.1f} KB")
    print(f"god_nodes_focus.png | {focus_nodes} nodes | {file_kb(FOCUS_PNG):.1f} KB")
    print(f"graph_interactive.html | {interactive_nodes} nodes | {file_kb(INTERACTIVE_HTML):.1f} KB")
    print(f"layout full={full_layout} ({full_seconds:.1f}s), focus={focus_layout}")
    print(f"god-node matches={len(matches)}/{len(GOD_NODES)}")


if __name__ == "__main__":
    main()
