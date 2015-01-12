Add this import
  <Import Project="$(MSBuildProjectDirectory)\..\..\ext\scripts\ilmerge\ilmerge.targets.xml" />

after this import
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />

in the project file. Then add a copy of the ilmerge.xml into your project or solution root path.