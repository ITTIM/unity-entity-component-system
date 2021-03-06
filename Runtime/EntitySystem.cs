namespace ElRaccoone.EntityComponentSystem {

  /// Base class for every entity system.
  public abstract class EntitySystem<EntitySystemType, EntityComponentType> : IEntitySystem
    where EntitySystemType : EntitySystem<EntitySystemType, EntityComponentType>, new()
    where EntityComponentType : EntityComponent<EntityComponentType, EntitySystemType>, new() {

    /// Defines whether this system is enabled.
    private bool isEnabled = false;

    /// Defines whether this component has been initialized.
    private bool isInitialized = false;

    /// An instance reference to the controller.
    public static EntitySystemType Instance;

    /// A list of the system's instantiated entity components.
    public System.Collections.Generic.List<EntityComponentType> entities = new System.Collections.Generic.List<EntityComponentType> ();

    /// The first instantiated entity compoent if this system.
    public EntityComponentType entity = null;

    /// Defines the number of instantiated entity components this system has.
    public int entityCount = 0;

    /// Defines whether the system has instantiated entity components.
    public bool hasEntities = false;

    /// Method invoked when the system will initialize.
    public virtual void OnInitialize () { }

    /// Method invoked when the system is initialized.
    public virtual void OnInitialized () { }

    /// Method invoked when the system updates, will be called every frame.
    public virtual void OnUpdate () { }

    // Method invoked when the system is drawing the gizmos, will be called
    // every gizmos draw call.
    public virtual void OnDrawGizmos () { }

    // Method invoked when the system is drawing the gui, will be called every
    // on gui draw call.
    public virtual void OnDrawGui () { }

    // Method invoked when the system becomes enabled.
    public virtual void OnEnabled () { }

    // Method invoked when the system becomes disabled.
    public virtual void OnDisabled () { }

    // Method invoked when an entity of this system is initializing.
    public virtual void OnEntityInitialize (EntityComponentType entity) { }

    // Method invoked when an entity of this system is initialized.
    public virtual void OnEntityInitialized (EntityComponentType entity) { }

    // Method invoked when an entity of this system becomes enabled.
    public virtual void OnEntityEnabled (EntityComponentType entity) { }

    // Method invoked when an entity of this system becomes disabled.
    public virtual void OnEntityDisabled (EntityComponentType entity) { }

    // Method invoked when an entity of this system will destroy.
    public virtual void OnEntityWillDestroy (EntityComponentType entity) { }

    // Method invoked before the system will update, return whether this system
    // should update. will be called every frame.
    public virtual bool ShouldUpdate () { return true; }

    /// Returns another component on an entity.
    public void GetComponentOnEntity<C> (EntityComponentType entity, System.Action<C> then) {
      var _entity = entity.GetComponent<C> ();
      if (_entity != null)
        then (_entity);
    }

    /// Returns another component on an entity.
    public C GetComponentOnEntity<C> (EntityComponentType entity) =>
      entity.GetComponent<C> ();

    /// Checks whether an entity has a specific component.
    public bool HasComponentOnEntity<C> (EntityComponentType entity) =>
      entity.GetComponent<C> () != null;

    /// Creates a new entity.
    public EntityComponentType CreateEntity () =>
      new UnityEngine.GameObject ("entity " + typeof (EntityComponentType).Name)
        .AddComponent<EntityComponentType> ();

    /// Clones an entity.
    public EntityComponentType CloneEntity (EntityComponentType entity) =>
      UnityEngine.Object.Instantiate (entity).GetComponent<EntityComponentType> ();

    /// Finds entities using a predicate match.
    public EntityComponentType[] MatchEntities (System.Predicate<EntityComponentType> match) =>
      this.entities.FindAll (match).ToArray ();

    /// Finds an entity using a predicate match.
    public EntityComponentType MatchEntity (System.Predicate<EntityComponentType> match) =>
      this.entities.Find (match);

    /// Starts a coroutine on this system.
    public UnityEngine.Coroutine StartCoroutine (System.Collections.IEnumerator routine) =>
      Controller.Instance.StartCoroutine (routine);

    /// Stops a given coroutine.
    public void StopCoroutine (System.Collections.IEnumerator routine) =>
      Controller.Instance.StopCoroutine (routine);

    /// Enables or disables this system.
    public void SetEnabled (bool isEnabled) =>
      Instance.isEnabled = isEnabled;

    /// Gets the enabled status of this system
    public bool GetEnabled () =>
      Instance.isEnabled;

    /// Internal method to set the instance reference. This method will
    /// be called after the controller and system initialization.
    [Internal]
    public void Internal_OnInitialize () =>
      Instance = Controller.Instance.GetSystem<EntitySystemType> ();

    /// Internal method to update the children of the system.
    [Internal]
    public void Internal_OnUpdate () {
      if (this.isInitialized == false) {
        this.OnInitialized ();
        if (this.isEnabled == true)
          this.OnEnabled ();
        this.isInitialized = true;
      }
      for (var _entityIndex = 0; _entityIndex < this.entityCount; _entityIndex++)
        this.entities[_entityIndex].Internal_OnUpdate ();
    }

    /// Internal method to add an entity's component to this system.
    [Internal]
    public void Internal_AddEntity (EntityComponentType component) {
      if (this.hasEntities == false)
        this.entity = component;
      this.entityCount++;
      this.hasEntities = true;
      this.entities.Add (component);
      this.OnEntityInitialize (component);
    }

    /// Internal method to remove an entity's component from this system.
    [Internal]
    public void Internal_RemoveEntry (EntityComponentType component) {
      this.entityCount--;
      this.hasEntities = this.entityCount > 0;
      this.OnEntityWillDestroy (component);
      this.entities.Remove (component);
      this.entity = this.hasEntities == false ? null : this.entities[0];
    }
  }
}
