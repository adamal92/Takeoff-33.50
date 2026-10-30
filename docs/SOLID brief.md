## Overall assessment

The project has **good foundations for a small demo**, but it doesn’t fully meet production-grade practices yet. The code is organized into sensible client components and server controller/service layers; TypeScript strict mode is enabled, React server state is separated from local interaction state, and the .NET code generally uses async operations and cancellation tokens.

I reviewed the client and server source, tests, build/configuration files, CI, and project documentation. This was a read-only review; I did not edit files or run tests.

## Highest-priority findings

1. **GeoJSON type validation can accept a value MongoDB rejects.** The parser compares geometry types case-insensitively in `GeoJsonGeometryParser.cs`, but the original type string is persisted unchanged. For example, `"polygon"` can pass application validation and then fail when MongoDB processes it. Validate exact canonical GeoJSON types or normalize the value before persistence. Coordinate ranges should also be checked so invalid longitude/latitude values produce a `400`, rather than a database error.

2. **The registered MongoDB singleton is bypassed.** `Program.cs` registers an `IMongoClient`, but `PolygonService.cs` and `MapObjectService.cs` each construct another client. Inject and reuse the registered client. Both services also create indexes synchronously in their constructors (`PolygonService.cs`, `MapObjectService.cs`); move that startup work to an explicit initialization step or deployment process.

3. **Create responses don’t point to an individual resource.** The `CreatedAtAction` calls in `PolygonsController.cs` and `ObjectsController.cs` target `GetAll`, whose route has no `{id}`. There is no `GET /{id}` endpoint, so the generated `Location` won’t identify the created feature. Add item-level GET routes or return a location that actually resolves to the resource.

4. **Selecting a table row is mouse-only.** `MapDataTable.tsx` and `MapDataTable.tsx` attach click handlers to `<tr>` elements, which aren’t keyboard-focusable controls. Use a button/control within the row, or provide equivalent keyboard interaction. This conflicts with WCAG’s keyboard-operability requirement.

5. **The API client’s TypeScript types aren’t validated at runtime.** `api.ts` asserts that parsed JSON is `T`; TypeScript can’t confirm the server actually returned that shape. The `undefined as T` in `api.ts` is another escape hatch. Prefer explicit response handling and runtime validation at this boundary.

## Other improvement areas

- **Server-side input rules are looser than the UI.** The request DTOs only use `[Required]` for names and symbol type (`CreatePolygonRequest.cs`, `CreateMapObjectRequest.cs`). They don’t enforce non-whitespace names, maximum lengths, or the client’s supported symbol values. Enforce shared business constraints at the API boundary.
- **Feature types are broader than the UI supports.** `symbolType` is plain `string` in `types.ts`, and the store accepts any string in `mapStore.ts`, despite only offering `marker` and `jeep`. A shared literal union (and matching server validation) would make this contract safer.
- **Collection reads are unbounded.** Both services retrieve the full collection with `ToListAsync` (`PolygonService.cs`, `MapObjectService.cs`). That’s acceptable for a small demo, but should become paginated if data can grow.
- **CI tests the client but doesn’t type-check/build it.** The workflow runs `pnpm test` in `ci.yml`; that does not run the `tsc` step in the client’s build script. Add `pnpm build` to CI. I also found no ESLint/React Hooks lint configuration or `.editorconfig`.

## SOLID summary

- **S — Single Responsibility:** Generally good separation into UI components, hooks, API client, controllers, and services. The map canvas combines map rendering, interaction, and selection focus, but that’s reasonable at this project size.
- **O — Open/Closed:** Mostly adequate for a small app; free-form symbol strings weaken the feature contract.
- **L — Liskov Substitution:** No significant inheritance hierarchy, so there’s little to assess here.
- **I — Interface Segregation:** The focused service interfaces are appropriately small.
- **D — Dependency Inversion:** Controllers depend on abstractions, but the MongoDB services bypass dependency injection by constructing their own clients. This is the clearest SOLID gap.

The API currently has no authentication or authorization, and its CORS rule is limited to loopback origins in `Program.cs`. That’s consistent with a local demo, but the API should not be exposed publicly without access control and deployment-specific CORS configuration.

These observations follow React’s [Rules of React](https://react.dev/reference/rules), the [TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/2/everyday-types.html), [Microsoft’s C# conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions), MongoDB’s [C# client guidance](https://www.mongodb.com/docs/drivers/csharp/current/connect/mongoclient/), and [WCAG keyboard guidance](https://www.w3.org/WAI/WCAG22/Understanding/keyboard.html).