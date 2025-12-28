# Function Extensions - Quick Reference

## One-Line Summary for Each Method

### Composition
| Method | Signature | Description |
|--------|-----------|-------------|
| `Compose` | `Func<T,TR> Compose<T,TK,TR>(Func<TK,TR> f, Func<T,TK> g)` | Math composition: f(g(x)) |
| `AndThen` | `Func<T,TR> AndThen<T,TK,TR>(Func<T,TK> f, Func<TK,TR> g)` | Chain functions: g(f(x)) |

### Currying
| Method | Signature | Description |
|--------|-----------|-------------|
| `Curry` (2) | `Func<T1,Func<T2,TR>> Curry<T1,T2,TR>(Func<T1,T2,TR>)` | Split 2-param to nested |
| `Curry` (3) | `Func<T1,Func<T2,Func<T3,TR>>> Curry<...>(Func<T1,T2,T3,TR>)` | Split 3-param to nested |
| `Curry` (4) | `Func<T1,Func<T2,Func<T3,Func<T4,TR>>>> Curry<...>(...)` | Split 4-param to nested |
| `Uncurry` (2) | `Func<T1,T2,TR> Uncurry<...>(Func<T1,Func<T2,TR>>)` | Merge nested to 2-param |
| `Uncurry` (3) | `Func<T1,T2,T3,TR> Uncurry<...>(Func<T1,Func<T2,Func<T3,TR>>>)` | Merge nested to 3-param |
| `Uncurry` (4) | `Func<T1,T2,T3,T4,TR> Uncurry<...>(...)` | Merge nested to 4-param |

### Partial Application
| Method | Signature | Description |
|--------|-----------|-------------|
| `Partial` (2→1) | `Func<T2,TR> Partial<T1,T2,TR>(Func<T1,T2,TR>, T1)` | Fix 1st arg of 2-param |
| `Partial` (3→2) | `Func<T2,T3,TR> Partial<...>(Func<T1,T2,T3,TR>, T1)` | Fix 1st arg of 3-param |
| `Partial` (3→1) | `Func<T3,TR> Partial<...>(Func<T1,T2,T3,TR>, T1, T2)` | Fix 1st,2nd of 3-param |
| `Partial` (4→3) | `Func<T2,T3,T4,TR> Partial<...>(Func<T1,T2,T3,T4,TR>, T1)` | Fix 1st arg of 4-param |
| `Partial` (4→2) | `Func<T3,T4,TR> Partial<...>(Func<T1,T2,T3,T4,TR>, T1, T2)` | Fix 1st,2nd of 4-param |
| `Partial` (4→1) | `Func<T4,TR> Partial<...>(Func<T1,T2,T3,T4,TR>, T1, T2, T3)` | Fix 1st,2nd,3rd of 4-param |

### Flip
| Method | Signature | Description |
|--------|-----------|-------------|
| `Flip` (2) | `Func<T2,T1,TR> Flip<T1,T2,TR>(Func<T1,T2,TR>)` | Reverse 2 params |
| `Flip` (3) | `Func<T2,T1,T3,TR> Flip<T1,T2,T3,TR>(Func<T1,T2,T3,TR>)` | Reverse first 2 of 3 |
| `Flip` (4) | `Func<T2,T1,T3,T4,TR> Flip<...>(Func<T1,T2,T3,T4,TR>)` | Reverse first 2 of 4 |

### Identity & Constant
| Method | Signature | Description |
|--------|-----------|-------------|
| `Identity` | `Func<T,T> Identity<T>()` | Returns input: x => x |
| `Constant` (1) | `Func<T,TR> Constant<T,TR>(TR value)` | Ignores input, returns value |
| `Constant` (0) | `Func<TR> Constant<TR>(TR value)` | Returns value: () => value |

### Apply & Invoke
| Method | Signature | Description |
|--------|-----------|-------------|
| `Apply` | `TR Apply<T,TR>(this T value, Func<T,TR> func)` | Reverse apply: value.Apply(f) |
| `Invoke` | `T Invoke<T>(this Func<T> func)` | Execute: func.Invoke() |

### Predicates
| Method | Signature | Description |
|--------|-----------|-------------|
| `Negate` | `Func<T,bool> Negate<T>(Func<T,bool>)` | Logical NOT: !predicate(x) |
| `And` | `Func<T,bool> And<T>(Func<T,bool>, Func<T,bool>)` | Logical AND: p1(x) && p2(x) |
| `Or` | `Func<T,bool> Or<T>(Func<T,bool>, Func<T,bool>)` | Logical OR: p1(x) \|\| p2(x) |
| `Xor` | `Func<T,bool> Xor<T>(Func<T,bool>, Func<T,bool>)` | Logical XOR: p1(x) ^ p2(x) |

### Actions
| Method | Signature | Description |
|--------|-----------|-------------|
| `AndThen` | `Action<T> AndThen<T>(Action<T>, Action<T>)` | Chain actions |
| `ToFunc` (1) | `Func<T,Unit> ToFunc<T>(Action<T>)` | Action → Func<Unit> |
| `ToFunc` (0) | `Func<Unit> ToFunc(Action)` | Action → Func<Unit> |

## Common Patterns

### Pipeline
```csharp
value.Apply(f1).Apply(f2).Apply(f3)
// or
f1.AndThen(f2).AndThen(f3)(value)
```

### Configuration
```csharp
var configured = func.Partial(arg1).Partial(arg2)
configured(finalArg)
```

### Validation
```csharp
var validator = rule1.And(rule2).And(rule3)
items.Where(validator)
```

### Composition
```csharp
var pipeline = parse.AndThen(validate).AndThen(transform)
```

## Cheat Sheet: F# to C# Mapping

| F# | C# (Roufe) | Example |
|----|------------|---------|
| `f >> g` | `f.AndThen(g)` | Forward pipe |
| `f << g` | `f.Compose(g)` | Backward pipe |
| `\|\>` | `.Apply()` or `.Pipe()` | Value pipe |
| Currying (auto) | `.Curry()` | Explicit curry |
| `not predicate` | `predicate.Negate()` | Negate |
| `f &&& g` | N/A | Use tuples |
| `f \|\|\| g` | N/A | Use Either/Result |
| `id` | `Identity<T>()` | Identity |
| `konst` | `Constant<T>(val)` | Constant |

## Performance Notes

- ✅ All methods use `AggressiveInlining`
- ✅ All methods use `AggressiveOptimization`
- ✅ Minimal allocations (only closure objects)
- ⚠️ Deep nesting can impact stack
- ⚠️ Extreme hot paths: consider direct calls

## Common Use Cases

1. **Data transformation pipelines**: Use `AndThen`
2. **Configuration builders**: Use `Partial`
3. **Validation rules**: Use predicate combinators
4. **API builders**: Use `Curry` + `Partial`
5. **Fluent interfaces**: Use `Apply`
6. **Side effects in monadic code**: Use `ToFunc`

