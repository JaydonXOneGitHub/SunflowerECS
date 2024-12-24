# SunflowerECS
 
SunflowerECS is an ECS* designed to be used in whatever kind of C# application you wish.

# Requirements
- .NET 8.0 or later

# How to Build
If using Visual Studio, open the solution. Then, press F6 or click Build>Build Solution:

![image](https://github.com/user-attachments/assets/8157cf46-2aaa-4f9c-b215-752d5cabe013)

If using a terminal, open the folder containing the solution. Then type the following:

![image](https://github.com/user-attachments/assets/da9c48dd-bfc5-4503-8577-fcf7ca41dbdb)


# How to Use

Create a Scene:

```Scene scene = new Scene();```

Create your custom component types.<br><br>

Create a custom system based on IUpdateSystem and/or IDrawSystem.

Example:

```scene.AddSystem(new BehaviourSystem());```

BehaviourSystem is a type built into this library.

In order to use it in your application, use:

```scene.UpdateBehaviour();```

and

```scene.DrawBehaviour();```

To use your own systems in the same manner, use:

```scene.UpdateGeneral();```

and

```scene.DrawGeneral();```

Create an Entity by doing this:

```Entity entity = scene.Create();```

The Entity in question will have a randomly generated ID.

Attach components by using:

```entity.AddComponent<YourComponentType>();```

You can store the instance by using:

```var yourComponent = entity.AddComponent<YourComponentType>();```

Alternatively, you can add an existing component by using:

```entity.AddComponent<YourComponentType>(yourComponent);```

To remove them, use:

```entity.RemoveComponent<YourComponentType>(yourComponent);```

To retrieve them, use:

```entity.GetComponent<YourComponentType>();```
















*not really
