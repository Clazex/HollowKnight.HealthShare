# [Health Share](https://github.com/Clazex/HollowKnight.HealthShare)

[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)](http://commitizen.github.io/cz-cli/)

A Hollow Knight mod to let some bosses use shared health and provide health sharing API.

Compatible with `Hollow Knight` 1.5.

Bosses affected includes Vengefly King, Mantis Lords, Sisters of Battle and God Tamer. Default disabled.

For using the API, check `SharedHealthManager` and `HealthShareUtil` class in which all public API are located.
Both hard and soft (via `ModInterop`) dependence is supported.
The built-in boss modifications provide examples of using the API.
