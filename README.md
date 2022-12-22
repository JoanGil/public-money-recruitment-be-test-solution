## GENERAL
- I've tried to keep the solution as simple as possible (not used Repository pattern cause I used EF InMemory)
- I focused my solution on using a clean architecture, services approach, KISS, Entity Framework

## MISSING PARTS
- Proper unit tests of all the methods, including controllers and services
- Proper validation of some of the functions

## UNIT TESTS
- Used a ImMemory Database. It is non relational test, so, some of the entities generated to populate the database, work in an isolated 
environment, but they don't necessarily are right as a whole.
- I know there are much more test that can be done (Controllers + more cases per function). I've created a bit of each Service to show how they would be implemented.

## HOW TO TEST THE API
- A Postman collection has been created to test it (included in the solution)


Questions are more than welcome.