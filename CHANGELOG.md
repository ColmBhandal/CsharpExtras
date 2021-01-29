# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]


### Changed
- [BREAKING] Move the ArrayOrientation enum to a new namespace (#9)
- [BREAKING] Rename main namespace from `CustomExtensions` to `Extensions` (#9)
- [BREAKING] The `TryGetSingleton` extension is now only available for collections with elements of type `class` (#49)
- [BREAKING] The `IMultiValueMap` has been renamed to `ISetValuedDictionary` and its package is different (#53)
- [BREAKING] Some renames & packages moved. E.g. the dictionary package has now been moved under map (#60)
- 2D one-based arrays are now enumerable (#45)
- Multi-value maps are now comparable using set-equality as the default equals method (#48)
- Project now treats all warnings as errors, minor changes made to fix all errors (#49)
- There is now a type `IListValuedDictionary` which is analogous to `ISetValuedDictionary` but for lists vs. sets (#53)
- Added variant dictionary wrapper type, allowing for subsets of dictionary functions to be treated with co/contra-variance (#60)
- Added functions for flattening 2D arrays to 1D arrays (#60)
- Added functions to convert arrays to dictionaries (#60)
- Added functions to find the last used row/column in 2D arrays (#75)

### Added
- Support for writing a 1D one-based array to a 2D one-based array (#9)
- Support for writing a 2D array to another array (#12)
- Unit tests for boolean extension (#27)
- New generic validated type class and some concrete implementations (#42)
- New `Exists` method on `IFileFacade`
- Added Curry Dictionary type (#82)

## [1.1.0] - 2020-06-08

### Added
- New extension methods for multi dimensional array (#6)
- Setup github action to automatically compile and test the project (#4)
- New transform method for multi-value maps (#5)
- Support for zipping 2D arrays (#3)

## [1.0.0] - 2020-04-04

Initial release
