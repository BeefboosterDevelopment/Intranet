using System;
using System.Web;
using System.Web.Caching;
namespace Beefbooster.Web
{

/*
    Inside a User Class - If you have a User class within your project, 
     you can even take it a step further and expose an instance of SessionCache as one of its properties. Here's an example:

   public class User
   {
      private int _userId;
      private SessionCache _cache;

      public SessionCache Cache
      {
         get { return _cache; }
      }
      
      //used internally to re-create a user from the database
      internal User(int userId)
      {
         _userId = userId;
         _cache = new SessionCache(_userId.ToString());
      }
   }

  
    Consumers can then use a very clean API to store and retrieve user-specific information from the cache:

    //store
    user.Cache.Insert("Roles", roles);
    //retrieve
    user.Cache["Roles"];

*/
    /// <summary>
    /// Summary description for SessionCache
    /// </summary>
    public class SessionCache
    {
    #region Fields and Properties
    private string _uniqueId;
    public object this[string key]
    {
     get { return HttpRuntime.Cache[CreateKey(key)]; }
     set { Insert(key, value); }
    }
    #endregion

    #region Constructors
    public SessionCache()
    { 
     // no web session - then use a GUID
     if (HttpContext.Current == null)
     {
        Init(Guid.NewGuid().ToString());
     }
     else
     {
        Init(HttpContext.Current.Session.SessionID);
     }
    }
    public SessionCache(string uniqueId)
    {
     Init(uniqueId);
    }
    private void Init(string uniqueId)
    {
     if (string.IsNullOrEmpty(uniqueId))
     {
        throw new ArgumentNullException("uniqueId");
     }
     _uniqueId = uniqueId;
    }
    #endregion

    #region Inserts
    public void Insert(string key, object data)
    {
     HttpRuntime.Cache.Insert(CreateKey(key), data);
    }

    public void Insert(string key, object data, CacheDependency dependency)
    {
     HttpRuntime.Cache.Insert(CreateKey(key), data, dependency);
    }
    //more inserts
    #endregion
    
    #region Deletes
    public void Remove(string key)
    {
        HttpRuntime.Cache.Remove(CreateKey(key));
    }
    #endregion

    #region Private Methods
    private string CreateKey(string key)
    {
     if (string.IsNullOrEmpty(key))
     {
        throw new ArgumentNullException("key");
     }
     return string.Format("{0}:{1}", key, _uniqueId);
    }
    #endregion
    }
}