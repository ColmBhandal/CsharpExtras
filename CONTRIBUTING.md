# How to contribute to CsharpExtras

## **Did you find a bug?**

* **Ensure the bug was not already reported** by searching on GitHub under [Issues](https://github.com/ColmBhandal/CsharpExtras/issues).

* If you're unable to find an open issue addressing the problem, [open a new one](https://github.com/ColmBhandal/CsharpExtras/issues/new?assignees=&labels=bug&template=bug_report.md&title=). Note the new issue should follow the "Bug Report" issue template in this case.

## **Did you write a patch that fixes a bug?**

* Open a new GitHub pull request with the patch.

* Ensure the PR description clearly describes the problem and solution. LInk the PR to the issue(s) that it solves e.g. by including the relevant issue number(s) in the PR description.

## **Do you intend to add a new feature or change an existing one?**

* [Open a new issue](https://github.com/ColmBhandal/CsharpExtras/issues/new?assignees=&labels=enhancement&template=feature_request.md&title=) on GitHub following the "Feature Request" template.

## **Do you have questions about the source code?**

* [Open a new issue](https://github.com/ColmBhandal/CsharpExtras/issues/new), type in your question and label it as "Question".

## Pull Request Guidelines

**IMPORTANT: Please do not create a Pull Request that is not linked to an issue.**

* The PR must solve one or more existing issues.

* Ensure any issues that this PR solves are linked to the PR.

* If you created any of those issues yourself, please ensure that a member of the project team has approved the issue before working on it. Approval will guarantee that the issue is sufficiently well written and that it constitutes an appropriate change.

## C# Coding Conventions

We typically follow these conventions when coding in most cases. We may request changes to your pull request based on the below. Please note that this doesn't always mean your code is "wrong", but it may be inconsistent relative to the conventions we've adopted. Where possible, we'll explain why we used certain conventions.

| Convenrtion | Explanation |
|---|---|
| Use Explicit Types (not `var`) | We always prefer explicit types vs `var`. We feel the code is easier to read when the type is right there. |
| Use Interface Types | Unless there's good reason to do so, avoid using raw classes. Use interfaces instead. This makes the code easily separable by the interface separation principle. So if you have a class `A` and a class `B` which uses `A`, rather than reference `A` directly from `B`, the recommendation is to create an interface `IA`, exposing only the public methods of `A` and then make `B` depend on `IA`.|
| Variable Naming | As far as possible, variables should be named so that the meaning of the variable can be inferred from the variable. name. You can always start coding with variables like `i` and `j` and then when it's time to commit your code, you can use your IDE to rename the variables to something more descriptive e.g. `rowIndex` and `columnIndex`.|

...More conventions will follow. Please be patient with us as we convert our coding experience to text...
