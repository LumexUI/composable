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

## Trying it out

There are two equivalent ways to use LumexUI (Experimental).

### 1) Manual copy (canonical approach)

1. Open the app/Registry/ folder in this repo
2. Copy the component folder you need (e.g. Field/)
3. Paste it into your Blazor project
4. Adjust namespaces if needed
5. Use and modify freely

This is the core model.
Everything else is optional.

### 2) Optional helper (convenience only)

There is a small helper tool that automates copying files:

```bash
dotnet tool install --global LumexUI.Cli
lumex add Button -o ./Components
```

The helper:

- only copies files
- adds no runtime dependency
- can be deleted immediately after use

If the helper disappeared tomorrow, nothing would break.

## Tailwind CSS

This approach is designed to work well with Tailwind CSS in Blazor:

- styles live next to markup
- no NuGet scanning issues
- no pre-generated CSS
- no hidden class names

You are expected to adapt classes to your own design system.

## Status

Experimental.

This is an exploration, not a committed roadmap.

Depending on whether this approach resonates and proves sustainable, the project may:

- evolve further
- change direction
- or stop entirely

## Feedback (important)

I’m not looking for:

- requests for missing components
- parity with existing libraries
- feature checklists

I am looking for answers to these questions:

1. Does this composability model make sense in Blazor?
2. Would you feel comfortable owning and modifying this code?
3. Does this reduce or increase mental overhead for you?

If you try it — even briefly — feedback is very welcome.

## Relationship to LumexUI (Classic)

This repository explores a different direction than the existing, package-based LumexUI.

The classic version remains available, but this experiment is intentionally not bound by its goals or constraints.

## Final note

This project exists to answer one question:

> Is a source-first, composable UI approach viable in the Blazor ecosystem?

If the answer is "no", that’s still a valuable outcome.
