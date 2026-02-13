# LumexUI Composable

**Composable, source-first UI patterns for Blazor.**

LumexUI Composable is an exploration of a different way to build UI in Blazor — inspired by modern, composable approaches (like shadcn/ui), but adapted to the realities of .NET, Razor, and Tailwind CSS.

This is not a full component library.
This is not a replacement for MudBlazor, Radzen, or other Swiss-knife frameworks.

It is an experiment.

## Why this exists

Blazor UI today mostly revolves around large, package-based component libraries. They are powerful, but they come with trade-offs:

- heavy abstractions
- limited composability
- difficult customization
- styling systems that fight Tailwind
- long-term lock-in to a framework's decisions

At the same time, modern frontend workflows increasingly favor:

- small, composable building blocks
- ownership of UI source code
- explicit structure over hidden magic

LumexUI Composable asks a simple question:

> What if Blazor UI components were copied into your project and fully owned by you?

## Core idea

- Components are source-first
- You copy the code into your app
- There is no runtime dependency
- You are expected to edit and adapt the components
- Composability is achieved via parts-as-components, not configuration flags

If you don't want to own UI source code, this project is not for you.

## What this is (and is not)

This is:

- a set of composable UI patterns
- small, intentional component systems
- a way to explore source ownership in Blazor
- an experiment in sustainable maintenance

This is not:

- a complete UI framework
- a component catalog
- a drop-in replacement for existing libraries
- a promise of long-term support

## Current scope

This repository currently focuses on a small number of component systens that demonstrate the approach clearly:

- Field — form field primitives (label, control, message, etc.)
- Dialog — composition for modals

The goal is to prove the **model**, not to cover every UI need.

## How components are structured

Each component lives in its own folder and is copied as a unit.

Example (Field):

```bash
Registry/Field/
  Field.razor
  FieldContent.razor
  FieldDescription.razor
  FieldError.razor
  FieldGroup.razor
  FieldLabel.razor
  FieldLegend.razor
  FieldSet.razor
  FieldTitle.razor
  FieldOrientation.cs
```

- The folder is the copy-paste unit
- Each file represents a part
- Only one file typically contains shared logic
- Everything else is small and boring by design

This replaces the "single JSX file" approach with a folder-as-system model that fits Razor.

### Example: Field composition

```razor
<FieldSet Class="w-full max-w-xs">
  <FieldGroup>
    <Field>
      <FieldLabel for="username">Username</FieldLabel>
      <Input id="username" type="text" placeholder="Max Leiter" />
      <FieldDescription>
        Choose a unique username for your account.
      </FieldDescription>
    </Field>
    <Field>
      <FieldLabel for="password">Password</FieldLabel>
      <FieldDescription>
        Must be at least 8 characters long.
      </FieldDescription>
      <Input id="password" type="password" placeholder="••••••••" />
    </Field>
  </FieldGroup>
</FieldSet>
```

The important part is not the API surface — it's that:

- each piece is explicit
- you can remove parts you don't need
- you can change markup, classes, or behavior freely

There is no "correct" usage beyond what makes sense for your app.

## Quick start (10 minutes)

To evaluate the model quickly:

1. Clone the repo and open it
2. Install the CLI tool to copy components into the project

```sh
dotnet tool install --global LumexUI.Cli   # installs the CLI tool globally
```

3. Start adding components

```sh
lumex add Button Dialog Input Field -o ./Components # this will add all 4 components from the registry
```

4. Use it in a page and experiment with composition

That's enough to understand the approach.

The CLI:

- only copies source files 
- adds no runtime dependency
- can be removed at any time

## Tailwind CSS

This approach is designed to work well with Tailwind CSS in Blazor:

- styles live next to markup
- no NuGet scanning issues
- no pre-generated CSS
- no hidden class names

You are expected to adapt classes to your own design system.

## Status

**⚠️ Experimental ⚠️**

This is an exploration, not a committed roadmap.

Depending on whether this approach resonates and proves sustainable, the project may:

- evolve further
- change direction
- or stop entirely

## Feedback (important)

This experiment only works if people actually try it.

If you're willing to spend 10 minutes testing it in a throwaway project, I'd especially appreciate:

- What felt awkward?
- What felt surprisingly clean?
- Did owning the source increase or decrease mental overhead?
- Did the parts-as-components model feel natural in Blazor?

Even short notes are valuable.
Please open an issue describing your experience — positive or negative.

I am not optimizing for feature count.
I am trying to understand whether this model is viable in practice.

## Relationship to LumexUI (Classic)

This repository explores a different direction than the existing, package-based LumexUI.

The classic version remains available, but this experiment is intentionally not bound by its goals or constraints.

## Final note

This project exists to answer one question:

> Is a source-first, composable UI approach viable in the Blazor ecosystem?

If the answer is "no", that's still a valuable outcome. But the only way to answer that is to try it.
