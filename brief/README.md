# IMS DS6 Core Team Tech Test

## Aim 
Using AWS serverless technologies, build a small API to provide VIN (Vehicle Identification Number) storage and decoding functionality.

## Requirements

A consumer of the API should be able to:
- import a vehicle by providing a VIN
- get information about a previously-imported vehicle by providing a VIN

Ensure that the json file (in the `/input` folder) can be provided to the API as an input.

Ensure that the output from the API matches the format in the output.example file in the `/output` folder.

## Guidelines

Whilst the implementation of this exercise is intentionally open-ended, we're looking for the following:

* Easily redeployable architecture
* Code that uses logical conventions
* Good unit test coverage
* the ability to explain decisions that you made in defining the solution.

Aim to spend no more than an hour on completing the task.

Whilst security is clearly a primary concern, securing the API is not a requirement for this exercise.

For more information on VINs and their make up, you can view the [Wikipedia entry for _Vehicle Identification Number_](https://en.wikipedia.org/wiki/Vehicle_identification_number).

If you don't have access to an AWS account, you can sign up for a free account on the [AWS Free Tier](https://aws.amazon.com/free/).

At IMS we aim to take a 'right tool for the job' approach to languages, so you can complete this exercise in any suitable language you choose. For reference, our current stack uses Node.js, Python, and .Net Core.