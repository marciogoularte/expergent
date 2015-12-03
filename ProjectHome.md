Expergent

Mission

To develop the leading open-source rete-based Business Rules Engine for the .net platform. The focus is on performance, usability, and rule management.

Scope of this project

  * Includes a faithful implementation of the RETE/UL algorithm as described by Robert B. Doorenbos.
  * Uses the SharpDevelop IDE and the Boo programming language for rule authoring and compilation.
  * Includes AlphaNode evaluators (Equals, notEquals, lessThan, etc.) to eliminate unnecessary alpha matches up-front.
  * Full range of builtin functions, including list functions.
  * Conflict Resolution strategies
  * Mutual Exclusion (mutex) definition and evaluation.
  * Overrides - where one rule can disable another based on priority.
  * Aggregate functions (sum, average, etc.)
  * Assert Business Objects as facts, and SET properties and/or INVOKE methods on them.
  * Special support for the Neo ORM solution, creating fact enabled business objects without using reflection.

High-level features

  * Extremely fast.
  * Written in C#, not ported from some other language.
  * Simple, but powerful rule authoring syntax.

Assumptions

  * .net and mono compatible.
  * Commercial support available.

Related resources

Here are a few alternatives for comparison...

  * NxBRE is the first open-source rule engine for the .NET platform and a lightweight Business Rules Engine (aka Rule-Based Engine)
  * SRE (Simple Rule Engine) is a lightweight forward chaining inference rule engine for .NET. Its 'simple' because of the simplicity in writing and understanding the rules written in XML, but this 'simple' engine can solve complex problems.
  * Drools.NET is a Business Rules Engine (BRE) based on Charles Forgy's Rete algorithm. Developers can now exploit a powerful Rule Engine through a completely managed .NET code base! Drools.NET is based on Jboss Rules, and comes with all the features of that Rules Engine.