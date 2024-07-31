# Unity GetComponent Performance Tests

This repository contains performance tests for Unity's `GetComponent` method. The tests measure the performance of getting components using different approaches, leveraging Unity's performance testing tools and custom stopwatch measurements. The tests are designed to work with both runtime and editor environments.

## Structure

The project is organized as follows:

- `Tests.Runtime`: Contains runtime performance tests.
- `Tests.Editor`: Contains editor performance tests.
- `Tests.Runtime.GetComponentPerformanceTestsBase`: Base class for common setup, teardown, and test logic.

## Prerequisites

- Unity 2020.1 or later.
- Unity Test Framework.
- Unity Performance Testing Extension.

## Setup

1. Clone this repository to your local machine.
2. Open the project in Unity.
3. Ensure the `ComponentTestData` scriptable object and required prefabs are properly set up in the `Resources` folder.

## Usage

### Running the Tests

To run the performance tests, follow these steps:

1. Open the Unity Test Runner (`Window -> General -> Test Runner`).
2. In the Test Runner window, select the `PlayMode` tab to run runtime tests or the `EditMode` tab to run editor tests.
3. Click the `Run All` button to execute all tests.

### Test Configuration

The tests use a `ComponentTestData` scriptable object to configure test parameters such as:

- `prefab`: The prefab to instantiate for testing. If the prefab is missing, a new `GameObject` is created and the required component is added.
- `warmUpCount`: The number of warm-up iterations before measuring.
- `iterations`: The number of iterations for the performance measurement.
- `measurementCount`: The number of measurement samples to take.
- `useStopWatch`: Whether to use a custom stopwatch for measurements.

### Adding New Tests

To add a new test:

1. Create a new test class in either the `Tests.Runtime` or `Tests.Editor` folder.
2. Inherit from `GetComponentPerformanceTestsBase`.
3. Add new test methods following the pattern in existing test classes.

Example:
```csharp
using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class NewComponentPerformanceTests : GetComponentPerformanceTestsBase
    {
        [UnityTest, Performance]
        public IEnumerator TestGetComponent_NewComponent()
        {
            yield return RunTestCoroutine("GetComponent<NewComponent>", typeof(NewComponent));
        }
    }
}
