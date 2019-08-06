#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UnityPackages.EntityComponentSystem {

  public static class Menus {

    [MenuItem ("Unity Packages/Entity Component System/Open repository")]
    private static void OpenRepository () {
      UnityEngine.Application.OpenURL ("https://github.com/unity-packages/entity-component-system");
    }

    [MenuItem ("Unity Packages/Entity Component System/Create new Controller")]
    private static void CreateNewController () {
      Utils.CreateFile (
        "Save new Controller",
        "Main",
        "Controller",
        "cs",
        "using UnityPackages.EntityComponentSystem;",
        "",
        "public class {{name}}Controller : ECS.Controller {",
        "\tpublic override void OnInitialize () {",
        "\t\tthis.RegisterSystems (",
        "\t\t\t// new Systems...",
        "\t\t);",
        "\t}",
        "",
        "\tpublic override void OnInitialized () { }",
        "",
        "\tpublic override void OnUpdate () { }",
        "",
        "\tpublic override void OnGUI () { }",
        "}"
      );
    }

    [MenuItem ("Unity Packages/Entity Component System/Create new Component")]
    private static void CreateNewComponent () {
      Utils.CreateFile (
        "Save new Component",
        "Item",
        "Component",
        "cs",
        "using UnityPackages.EntityComponentSystem;",
        "using UnityEngine;",
        "",
        "public class {{name}}Component : ECS.Component<{{name}}Component, {{name}}System> {",
        "\t// [ECS.Protected] public bool myProtectedBool;",
        "\t// [ECS.Reference] public Transform myReferencesTransform;",
        "}"
      );
    }

    [MenuItem ("Unity Packages/Entity Component System/Create new System")]
    private static void CreateNewSystem () {
      Utils.CreateFile (
        "Save new System",
        "Item",
        "System",
        "cs",
        "using UnityPackages.EntityComponentSystem;",
        "",
        "public class {{name}}System : ECS.System<{{name}}System, {{name}}Component> {",
        "\tpublic override void OnInitialize () { }",
        "",
        "\tpublic override void OnUpdate () {",
        "\t\tforeach (var _entity in this.entities) {",
        "\t\t\t// use the entity...",
        "\t\t}",
        "\t}",
        "",
        "\tpublic override void OnDrawGizmos () { }",
        "",
        "\tpublic override void OnGUI () { }",
        "",
        "\tpublic override void OnEntityInitialize ({{name}}Component entity) { }",
        "",
        "\tpublic override void OnEntityStart ({{name}}Component entity) { }",
        "",
        "\tpublic override void OnEntityEnabled ({{name}}Component entity) { }",
        "",
        "\tpublic override void OnEntityDisabled ({{name}}Component entity) { }",
        "",
        "\tpublic override void OnEntityWillDestroy ({{name}}Component entity) { }",
        "}"
      );
    }
  }
}

#endif
