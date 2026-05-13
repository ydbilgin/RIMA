# Unity Cleanup Task — S66

Execute every step below completely.

## Step 1: BaseMobBehavior OnDestroy token leak fix
- Find Assets/Scripts/Combat/BaseMobBehavior.cs (or search for it)
- In OnDestroy, add: if this mob holds an AttackToken, call AttackTokenManager.Instance.ReturnToken() before destroying
- Check AttackTokenManager.cs for the correct return method signature

## Step 2: Dead code audit and removal
Search Assets/Scripts/ for and remove:
- Private methods that are never called anywhere in the project
- Commented-out code blocks (5+ consecutive lines)
- `#if false` blocks
- Empty method stubs with only a comment or TODO body
- `Debug.Log("placeholder")` / `Debug.Log("TODO")` / `Debug.Log("test")` lines

## Step 3: Unused using statements
- Remove unused `using` directives from any scripts touched in Steps 1-2

## Output
- Commit all changes: `chore(cleanup): dead code removal + BaseMobBehavior OnDestroy token leak fix`
- List changed files in the commit body
