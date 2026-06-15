# 3 native graphify figuru, community'ler KOD-KLASORUNE gore ISIMLI (0 token).
import json, os
from collections import Counter
import networkx as nx
from graphify.build import build_from_json
from graphify.cluster import cluster
from graphify.export import to_html

ast = json.loads(open('.graphify_extract.json', encoding='utf-8').read())
G = build_from_json(ast)

def folder_of(path, depth=2):
    p = (path or '').replace('\\', '/')
    i = p.find('Assets/Scripts/')
    seg = (p[i + len('Assets/Scripts/'):] if i >= 0 else p).split('/')
    folders = [s for s in seg[:-1] if s]
    return '/'.join(folders[:depth]) if folders else 'root'

def name_communities(graph, comms):
    """comms: {cid: iterable(node)} -> {cid: 'Folder/Sub'} (baskin klasor, cakismada #2 ekle)."""
    raw = {}
    for cid, members in comms.items():
        paths = []
        for n in members:
            d = graph.nodes[n] if graph.has_node(n) else {}
            paths.append(folder_of(d.get('source_file') or n))
        raw[cid] = Counter(paths).most_common(1)[0][0] if paths else 'misc'
    # ayni isim birden fazla community'de -> en buyuk olan ismi alir, digerleri "(2)" vb.
    seen = {}
    out = {}
    order = sorted(comms, key=lambda c: -len(list(comms[c])))
    for cid in order:
        nm = raw[cid]
        seen[nm] = seen.get(nm, 0) + 1
        out[cid] = nm if seen[nm] == 1 else f'{nm} ({seen[nm]})'
    return out

def emit(graph, comms, path, tag):
    labels = name_communities(graph, comms)
    to_html(graph, comms, path, community_labels=labels or None)
    top = Counter(labels.values())
    print(f'{tag}: {graph.number_of_nodes()} nodes, {len(comms)} comms ->', path)

# 1) DENSE backbone (symbol-level, degree>=3 <5000)
deg = dict(G.degree())
keep = None
for thr in [2, 3, 4, 5, 6, 8, 10]:
    k = [n for n, d in deg.items() if d >= thr]
    if len(k) < 4800:
        keep = k; break
subG = G.subgraph(keep).copy()
emit(subG, cluster(subG), 'graphify-out/graph.html', 'DENSE')

# 2) FILE-LEVEL (symbol -> source_file)
node_file = {n: d.get('source_file') for n, d in G.nodes(data=True) if d.get('source_file')}
F = nx.Graph()
for sf in set(node_file.values()):
    F.add_node(sf, label=os.path.basename(sf))
for u, v in G.edges():
    fu, fv = node_file.get(u), node_file.get(v)
    if fu and fv and fu != fv:
        if F.has_edge(fu, fv): F[fu][fv]['weight'] += 1
        else: F.add_edge(fu, fv, weight=1)
F_core = F.subgraph([n for n in F if F.degree(n) > 0]).copy()  # izole node'lari at
emit(F_core, cluster(F_core), 'graphify-out/graph_files.html', 'FILES')

# 3) CLOSEUP: editor hub DirectorMode.cs + komsular
target = next((n for n in F if os.path.basename(n) == 'DirectorMode.cs'), None)
if target is None:
    fdeg = dict(F.degree(weight='weight')); target = max(fdeg, key=fdeg.get)
neigh = sorted(F.neighbors(target), key=lambda x: F[target][x].get('weight', 1), reverse=True)[:28]
C = F.subgraph([target] + neigh).copy()
emit(C, cluster(C), 'graphify-out/graph_closeup.html', 'CLOSEUP=' + os.path.basename(target))
print('DONE')
