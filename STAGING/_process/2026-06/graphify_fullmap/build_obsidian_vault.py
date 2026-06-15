# Obsidian vault'u MEVCUT .graphify_extract.json'dan uretir (re-parse YOK, 0 token).
# build_ast_map.py ile ayni import/akis; sadece export adimi = to_obsidian + to_canvas.
import json, os
from pathlib import Path
from graphify.build import build_from_json
from graphify.cluster import cluster, score_all
from graphify.export import to_obsidian, to_canvas

ast = json.loads(Path('.graphify_extract.json').read_text())
G = build_from_json(ast)
communities = cluster(G)
cohesion = score_all(G, communities)
labels = {cid: 'Community ' + str(cid) for cid in communities}

obsidian_dir = 'graphify-out/obsidian'
os.makedirs(obsidian_dir, exist_ok=True)

n = to_obsidian(G, communities, obsidian_dir, community_labels=labels or None, cohesion=cohesion)
print('Obsidian vault:', n, 'notes in', obsidian_dir)
to_canvas(G, communities, obsidian_dir + '/graph.canvas', community_labels=labels or None)
print('Canvas:', obsidian_dir + '/graph.canvas')
print('NODES', G.number_of_nodes(), 'EDGES', G.number_of_edges(), 'COMMUNITIES', len(communities))
print('DONE')
