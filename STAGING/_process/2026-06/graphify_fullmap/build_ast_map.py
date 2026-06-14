import json, os
from pathlib import Path
from graphify.extract import collect_files, extract
from graphify.build import build_from_json
from graphify.cluster import cluster, score_all
from graphify.analyze import god_nodes, surprising_connections
from graphify.report import generate
from graphify.export import to_json, to_html

INPUT = r'F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts'

detect = json.loads(Path('.graphify_detect.json').read_text())
code_files = []
for f in detect.get('files', {}).get('code', []):
    p = Path(f)
    code_files.extend(collect_files(p) if p.is_dir() else [p])
print('code_files', len(code_files))

ast = extract(code_files)
Path('.graphify_extract.json').write_text(json.dumps(ast))
print('AST nodes', len(ast.get('nodes', [])), 'edges', len(ast.get('edges', [])))

G = build_from_json(ast)
communities = cluster(G)
cohesion = score_all(G, communities)
gods = god_nodes(G)
surprises = surprising_connections(G, communities)
labels = {cid: 'Community ' + str(cid) for cid in communities}
tokens = {'input': 0, 'output': 0}

os.makedirs('graphify-out', exist_ok=True)
report = generate(G, communities, cohesion, labels, gods, surprises, detect, tokens, INPUT)
Path('graphify-out/GRAPH_REPORT.md').write_text(report, encoding='utf-8')
to_json(G, communities, 'graphify-out/graph.json')

n = G.number_of_nodes()
print('NODES', n, 'EDGES', G.number_of_edges(), 'COMMUNITIES', len(communities), 'GODS', len(gods))
if n == 0:
    print('ERROR: empty graph (C# AST may be unsupported)')
elif n <= 5000:
    to_html(G, communities, 'graphify-out/graph.html', community_labels=labels or None)
    print('HTML written: graphify-out/graph.html')
else:
    print('HTML skipped (>5000 nodes) — use Obsidian/filter')
print('DONE')
