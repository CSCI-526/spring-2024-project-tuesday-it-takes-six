# It Takes Six

![Super-Linter](https://github.com/CSCI-526/spring-2024-project-tuesday-it-takes-six/actions/workflows/super-lint.yml/badge.svg)

## Links
- Trello: https://trello.com/b/RbDbHsST/it-takes-six-task-board-csci-526
- Deployment Repo: https://github.com/keyi6/it-takes-six-deployment/
- Deployment List: http://keyi6.github.io/it-takes-six-deployment/

## Level Design Suggestions
- the vertical gap <= 2
- the horizontal gap <= 4
- the spiky floor length <= 7


## Code Structures
### Data Manage
Codes related to data is under `Assets/Scripts/Data`. `GlobalData` is a static class that store all the global data shared between scenes and scripts.

When adding data in `GlobalData`, please strictly control the access to read and write the data. Updating value by an explicit setter function is strongly recommended. Please DO NOT just make variables public and assign value to it outside `GlobalData` class.

If the logics is too heavy, please create a class to handle the logics. Please keep the `GlobalData` as SHORT as possible.


### Utils
Common util functions should be placed under `Assets/Scripts/Utils.cs`.


### Controller
Every script that needs to be attached to a game object should be in this folder.

### UI Controller
Every script that needs to be attached to a UI element should be in this folder.

### namespace file
It is a place for put our game related terms, definitions, data structure and etc.
