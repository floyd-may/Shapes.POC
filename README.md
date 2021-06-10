# Proof-of-Concept for Shapes fluent API

So the general idea I'm going with here is to data-drive the code generation relationships between a given shape (triangle, polygon, ellipse)
and the features it supports, so that you can (eventually) just change the details of what features are supported by what shapes as you add
features and/or support for those features to different kinds of shapes. I know there are also some subtleties where certain sets of parameters
need to travel together, which is why this setup explicity lays out what combinations of features are supported.

## Design Goals

### Make invalid calls cause compiler errors

I wanted someone wayfinding via Intellisense/autocomplete to know what is and isn't supported based on the shape of the API. If the compiler doesn't let you do it, it's not supported.

### Make it easy to add support in the fluent API for new features piecemeal

It shouldn't be a crapton of tedious manual effort to add a feature to a given shape. The codegen for the fluent API should make it _Just Happen™_.

### Leave the door open for performance

Performance and developer ergonomics are often at odds, and this is definitely one of those cases. However, I think there's a light at the end of this tunnel. If you consider a code sample like this:

```csharp
Shapes.Triangle.Dashed().StrokeColor(0xff0000).Draw(p1, p2, p3);
```

As you've already discovered, this is going to be way slow compared to the static methods.
You're not going to get better performance than the static methods, which are heckin'
`e^nyoom` **screaming fast** *. However, there are some niceties that might be worth
considering. If you've got bajillions of triangles to draw, the fluent API
ought to support this kind of setup:

```csharp
var trianglerizeratorThingy = Shapes.Triangle.Dashed().StrokeColor(0xff0000);

foreach(var (p1, p2, p3) in coords)
{
    trianglerizeratorThingy.Draw(p1, p2, p3);
}
```
And I _think_ what's inside the loop ought to perform similarly to your static methods.

\* _That's `e^ñoom` if you're coding in Spanish_