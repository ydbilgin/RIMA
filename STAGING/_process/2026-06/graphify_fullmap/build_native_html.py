# graphify NATIVE to_html — backbone (degree-filtered <5000), 0 token.
# to_html'in 5000 hard-cap'i var; dusuk-degree yaprak node'lari atip backbone'u native render ediyoruz.
import json
from pathlib import Path
from graphify.build import build_from_json
from graphify.cluster import cluster
from graphify.export import to_html

ast = json.loads(Path('.graphify_extract.json').read_text())
G = build_from_json(ast)
communities = cluster(G)
labels = {cid: 'Community ' + str(cid) for cid in communities}

deg = dict(G.degree())
chosen = None
for thr in [2, 3, 4, 5, 6, 8, 10, 12, 15, 20, 25]:
    keep = [n for n, d in deg.items() if d >= thr]
    if len(keep) < 4800:
        chosen = (thr, keep)
        break
thr, keep = chosen
keepset = set(keep)
subG = G.subgraph(keep).copy()

# communities'i alt-grafa indir (sadece kalan node'lar)
sub_comm = {cid: [n for n in members if n in keepset] for cid, members in communities.items()}
sub_comm = {cid: m for cid, m in sub_comm.items() if m}
sub_labels = {cid: labels[cid] for cid in sub_comm}

out = 'graphify-out/graph.html'
to_html(subG, sub_comm, out, community_labels=sub_labels or None)
print('NATIVE graph.html | degree>=', thr, '|', subG.number_of_nodes(), 'nodes', subG.number_of_edges(), 'edges |', len(sub_comm), 'communities')
print('DONE')
