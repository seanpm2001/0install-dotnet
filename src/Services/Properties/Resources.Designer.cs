﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroInstall.Services.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZeroInstall.Services.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Feed: {0}
        ///- Valid signature from {1}
        ///- {2}
        ///Do you want to trust this key to sign feeds from &apos;{3}&apos;?
        ///.
        /// </summary>
        internal static string AskKeyTrust {
            get {
                return ResourceManager.GetString("AskKeyTrust", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The resulting command-line is empty..
        /// </summary>
        internal static string CommandLineEmpty {
            get {
                return ResourceManager.GetString("CommandLineEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command for &apos;{0}&apos; not specified..
        /// </summary>
        internal static string CommandNotSpecified {
            get {
                return ResourceManager.GetString("CommandNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;environment&gt; bindings must contain either a &apos;value&apos; or an &apos;insert&apos; attribute..
        /// </summary>
        internal static string EnvironmentBindingValueInvalid {
            get {
                return ResourceManager.GetString("EnvironmentBindingValueInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading catalog.
        /// </summary>
        internal static string ErrorLoadingCatalog {
            get {
                return ResourceManager.GetString("ErrorLoadingCatalog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error saving the trust database.
        /// </summary>
        internal static string ErrorSavingTrustDB {
            get {
                return ResourceManager.GetString("ErrorSavingTrustDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running external solver.
        /// </summary>
        internal static string ExternalSolverRunning {
            get {
                return ResourceManager.GetString("ExternalSolverRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feed &apos;{0}&apos; is not cached and Zero Install is currently in off-line mode..
        /// </summary>
        internal static string FeedNotCachedOffline {
            get {
                return ResourceManager.GetString("FeedNotCachedOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; could not be located in the feed cache..
        /// </summary>
        internal static string FeedNotInCache {
            get {
                return ResourceManager.GetString("FeedNotInCache", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feed &apos;{0}&apos; was not signed with any trusted keys..
        /// </summary>
        internal static string FeedNoTrustedSignatures {
            get {
                return ResourceManager.GetString("FeedNoTrustedSignatures", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is a local file. It should be a HTTP(S) URL..
        /// </summary>
        internal static string FeedUriLocal {
            get {
                return ResourceManager.GetString("FeedUriLocal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &lt;interface&gt; uri attribute (&apos;{0}&apos;) does not match the URL the feed was downloaded from (&apos;{1}&apos;)..
        /// </summary>
        internal static string FeedUriMismatch {
            get {
                return ResourceManager.GetString("FeedUriMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &lt;interface&gt; uri attribute missing. Should be &apos;{0}&apos;..
        /// </summary>
        internal static string FeedUriMissing {
            get {
                return ResourceManager.GetString("FeedUriMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Problem fetching &apos;{0}&apos;..
        /// </summary>
        internal static string FetcherProblem {
            get {
                return ResourceManager.GetString("FetcherProblem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Illegal character in {0} binding name..
        /// </summary>
        internal static string IllegalCharInBindingName {
            get {
                return ResourceManager.GetString("IllegalCharInBindingName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Feeds must specify the URI they originated from in order to be importable..
        /// </summary>
        internal static string ImportNoSource {
            get {
                return ResourceManager.GetString("ImportNoSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing name in {0} binding..
        /// </summary>
        internal static string MissingBindingName {
            get {
                return ResourceManager.GetString("MissingBindingName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No interface was specified..
        /// </summary>
        internal static string MissingInterfaceUri {
            get {
                return ResourceManager.GetString("MissingInterfaceUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to download {0} because Zero Install is in offline mode..
        /// </summary>
        internal static string NoDownloadInOfflineMode {
            get {
                return ResourceManager.GetString("NoDownloadInOfflineMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key information server was unable to provide any additional information about this key..
        /// </summary>
        internal static string NoKeyInfoServerData {
            get {
                return ResourceManager.GetString("NoKeyInfoServerData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No manifest digest for implementation &apos;{0}&apos; found..
        /// </summary>
        internal static string NoManifestDigest {
            get {
                return ResourceManager.GetString("NoManifestDigest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No retrieval method found for implementation &apos;{0}&apos; found..
        /// </summary>
        internal static string NoRetrievalMethod {
            get {
                return ResourceManager.GetString("NoRetrievalMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The downloaded feed is older than the version already in the cache. We will keep using the newer version.
        ///Feed URI: {0}
        ///Old time: {1}
        ///New time: {2}.
        /// </summary>
        internal static string ReplayAttack {
            get {
                return ResourceManager.GetString("ReplayAttack", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to download {0}. Trying to get from feed mirror instead..
        /// </summary>
        internal static string TryingFeedMirror {
            get {
                return ResourceManager.GetString("TryingFeedMirror", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to cache the downloaded application catalog..
        /// </summary>
        internal static string UnableToCacheCatalog {
            get {
                return ResourceManager.GetString("UnableToCacheCatalog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse feed downloaded from {0}..
        /// </summary>
        internal static string UnableToParseFeed {
            get {
                return ResourceManager.GetString("UnableToParseFeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse key information for &apos;{0}&apos;..
        /// </summary>
        internal static string UnableToParseKeyInfo {
            get {
                return ResourceManager.GetString("UnableToParseKeyInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to retrieve key information for &apos;{0}&apos;..
        /// </summary>
        internal static string UnableToRetrieveKeyInfo {
            get {
                return ResourceManager.GetString("UnableToRetrieveKeyInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not a known ID in the {1} package manager..
        /// </summary>
        internal static string UnknownPackageID {
            get {
                return ResourceManager.GetString("UnknownPackageID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feed is signed with untrusted keys!.
        /// </summary>
        internal static string UntrustedKeys {
            get {
                return ResourceManager.GetString("UntrustedKeys", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for download to complete.
        /// </summary>
        internal static string WaitingForDownload {
            get {
                return ResourceManager.GetString("WaitingForDownload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The working directory has already been changed by a previous command..
        /// </summary>
        internal static string WorkingDirAlreadyChanged {
            get {
                return ResourceManager.GetString("WorkingDirAlreadyChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The working directory contains an invalid path (potentially a security risk)..
        /// </summary>
        internal static string WorkingDirInvalidPath {
            get {
                return ResourceManager.GetString("WorkingDirInvalidPath", resourceCulture);
            }
        }
    }
}
