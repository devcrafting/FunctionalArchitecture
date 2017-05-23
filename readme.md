# Functional architecture experiments

The idea behind "functional architecture" is to have a pure "functional core" and an "imperative shell", pushing side effects to the border, removing them from the core (the domain, where rules are enforced) to make it more deterministic.

## The domain and proposed model

The code is based on a (subset of) online shopping domain. For the purpose of these experiments, I only focus on the use case of adding a product to cart with the following rules :

* Check enough stock then reserve temporarily (let's imagine there is a timer service that remove outdated reservations)
* Do not allow accumulated quantity (in the cart) for each product to be more than 10 (given it is possible to add a product several times, and cumulated quantity is 0)

I propose the following model (with DDD in mind) :

* ProductStock aggregate to enforce the first rule
* Cart aggregate to enforce the second one

## Common technical assumptions

In any of the examples, I will rely on these assumptions:

* (sort of MVC) controller end point
* some data store somewhere (but not implemented => it is not the subject)

## Hexagonal architecture with OOP

A.K.A. Onion architecture or Clean architecture...

The first step I propose is to setup an hexagonal architecture, with different flavors. It is a first step towards a functional architecture since the idea is to only rely on abstracted input and output adapters in the core.

### "Ifs and primitives" implementation

Here I propose to just rely on primitives return types from the core.

Drawbacks:

* Check "null" on productStock.MakeATemporaryReservation return => failure very implicit
* Check true/false on cart.Add => no information on why it fails
* Cascading "ifs"
* Pure and impure (Get/Save) functions alternate
* Business logic to stop/continue according there are errors or not
* Duplicate conditionals between aggregates functions and "ifs"

### "Exceptions" implementation

Then I propose to rely on exceptions for error cases.

Advantages :

* No more check on null/bool => more explicit with clear exceptions derived from BusinessException
* No more cascading "ifs"
* No more stop/continue business logic, try/catch is explicit
* Less duplicate conditionals

Drawbacks :

* Pure and impure (Get/Save) functions alternate
* Exceptions should not be used to handle business errors (these are expected, exceptions are more for unexpected cases)

### "Continuation" implementation

With "exceptions" implementation we see a pattern to continue execution: continues on success, stop and report on errors. Instead of exceptions, we can then rely on an Either generic type that represents success with a first type (special case of void with EitherVoid) OR error with a second type. Either has a ContinueWith method.

Advantages :

* Same as "exceptions" implementation
* But without using exceptions :)

Drawbacks :

* Syntax a bit heavy

We could use Either type slightly modified to use it with "if" implementation, removing the primitives drawbacks.

NB: for C# developers, perhaps, it reminds you the Task API, it is very close. Task implement some sort of continuation expression. For JS developers, it reminds use of callbacks, it is the same.
