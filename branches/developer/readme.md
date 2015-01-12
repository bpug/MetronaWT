# Build Scripts
Beschreibung der Build Scripts im Root-Verzeichnis der Solution.

## Allgemein
### Verzeichnisse
Nach einem erfolgreichen Build gliedert sich die Struktur der Verzeichnisse wir folgt:
```
    {root_directory}
    |-> _artifacts
    |   |-> {current_version}
    |   |   |-> binaries
    |   |   |   |-> {project_name}.{current_version}.zip
    |   |   |-> setup
    |   |   |   |-> {project_name}.{current_version}.exe
    |   |   |   |-> {project_name}.{current_version}.msi
    |-> _build
    |   |-> bin
    |   |   |-> release
    |   |   |   |-> {build_output}
    |   |   |   |-> ...
    |   |-> msbuild.log
```

