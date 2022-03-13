# [生命共享](https://github.com/Clazex/HollowKnight.HealthShare)

[![Commitizen 友好](https://img.shields.io/badge/commitizen-友好-brightgreen.svg)](http://commitizen.github.io/cz-cli/)

一个让一些 Boss 共享生命值，并提供生命值共享 API 的《空洞骑士》模组

适用于 `空洞骑士` 1.5。

影响的 Boss 包括反击蝇之王，骨钉兄弟奥罗 & 马托，螳螂领主，战斗姐妹以及神之驯服者。默认关闭。

若要使用 API，请查看 `SharedHealthManager` 与 `HealthShareUtil` 类，所有公共 API 均在其中。
硬依赖和软依赖（通过 `ModInterop`）均受支持。
内置的 Boss 修改提供了使用 API 的范例。
