import json, sys
from collections import deque
FLOOR=set('.PeCB')
EXPECTED_COUNTS={'Combat':7,'CombatLarge':2,'Elite':2,'Boss':1,'Chest':2,'Corridor':1}
def validate_room(r):
    rid,w,h,g=r['roomId'],r['width'],r['height'],r['grid']
    if len(g)!=h: raise ValueError(f'{rid}: height mismatch')
    bad=[(i,len(row)) for i,row in enumerate(g) if len(row)!=w]
    if bad: raise ValueError(f'{rid}: width mismatch {bad}')
    if sum(row.count('P') for row in g)!=1: raise ValueError(f'{rid}: expected exactly one P')
    for y,row in enumerate(g):
        for x,c in enumerate(row):
            if c not in FLOOR and c!=' ': raise ValueError(f'{rid}: invalid char {c!r} at {(x,y)}')
    for d in r['doors']:
        x,y=d['x'],d['y']
        if d['dir'] not in {'N','E','W'}: raise ValueError(f'{rid}: invalid door dir {d}')
        if d['dir']=='N' and y!=0: raise ValueError(f'{rid}: N door must be y=0')
        if d['dir']=='W' and x!=0: raise ValueError(f'{rid}: W door must be x=0')
        if d['dir']=='E' and x!=w-1: raise ValueError(f'{rid}: E door must be x=width-1')
        if not (0<=x<w and 0<=y<h) or g[y][x] not in FLOOR: raise ValueError(f'{rid}: door is not on floor {d}')
    walk=[(x,y) for y,row in enumerate(g) for x,c in enumerate(row) if c in FLOOR]
    q=deque([walk[0]]); seen={walk[0]}
    while q:
        x,y=q.popleft()
        for dx,dy in ((1,0),(-1,0),(0,1),(0,-1)):
            nx,ny=x+dx,y+dy
            if 0<=nx<w and 0<=ny<h and (nx,ny) not in seen and g[ny][nx] in FLOOR:
                seen.add((nx,ny)); q.append((nx,ny))
    if len(seen)!=len(walk): raise ValueError(f'{rid}: disconnected floor')
def main(path):
    rooms=json.load(open(path,encoding='utf-8'))
    counts={}
    for r in rooms:
        validate_room(r); counts[r['roomType']]=counts.get(r['roomType'],0)+1
    if counts!=EXPECTED_COUNTS: raise ValueError(f'counts mismatch: {counts}')
    print(f'OK: {len(rooms)} rooms validated.')
if __name__=='__main__':
    if len(sys.argv)!=2:
        print('Usage: python tools/validate_rooms.py rooms/rima_shattered_keep_rooms_large.json'); sys.exit(1)
    main(sys.argv[1])
