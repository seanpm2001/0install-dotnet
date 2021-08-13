﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroInstall.Store.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZeroInstall.Store.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Copying files.
        /// </summary>
        internal static string CopyFiles {
            get {
                return ResourceManager.GetString("CopyFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The subdirectories in this cache are protected from unintended modification using NTFS Permissions.
        ///As a side-effect this prevents them from being deleted using the Windows Explorer.
        ///
        ///If you wish to delete a single entry you can use this command line:
        ///0install store remove IMPLEMENTATION-ID
        ///
        ///If you wish to delete this entire directory you can use this command line:
        ///0install store purge {0}
        ///
        ///Alternatively you can use the graphical user interface for managing the caches:
        ///0install store manage.
        /// </summary>
        internal static string DeleteInfoFileContent {
            get {
                return ResourceManager.GetString("DeleteInfoFileContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How to delete.
        /// </summary>
        internal static string DeleteInfoFileName {
            get {
                return ResourceManager.GetString("DeleteInfoFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting directory {0}.
        /// </summary>
        internal static string DeletingDirectory {
            get {
                return ResourceManager.GetString("DeletingDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting implementation {0}.
        /// </summary>
        internal static string DeletingImplementation {
            get {
                return ResourceManager.GetString("DeletingImplementation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting obsolete files.
        /// </summary>
        internal static string DeletingObsoleteFiles {
            get {
                return ResourceManager.GetString("DeletingObsoleteFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An implementation&apos;s manifest hash does not match the expected value..
        /// </summary>
        internal static string DigestMismatch {
            get {
                return ResourceManager.GetString("DigestMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actual value: {0}.
        /// </summary>
        internal static string DigestMismatchActualDigest {
            get {
                return ResourceManager.GetString("DigestMismatchActualDigest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actual manifest: {0}.
        /// </summary>
        internal static string DigestMismatchActualManifest {
            get {
                return ResourceManager.GetString("DigestMismatchActualManifest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected value: {0}.
        /// </summary>
        internal static string DigestMismatchExpectedDigest {
            get {
                return ResourceManager.GetString("DigestMismatchExpectedDigest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected manifest: {0}.
        /// </summary>
        internal static string DigestMismatchExpectedManifest {
            get {
                return ResourceManager.GetString("DigestMismatchExpectedManifest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading the trust database. Reverting to default values..
        /// </summary>
        internal static string ErrorLoadingTrustDB {
            get {
                return ResourceManager.GetString("ErrorLoadingTrustDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage of the feed mirror, which is required for search, is disabled.
        ///Run &apos;0install config feed_mirror default&apos; to enable it..
        /// </summary>
        internal static string FeedMirrorDisabled {
            get {
                return ResourceManager.GetString("FeedMirrorDisabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; could not be located in the feed cache. Looked for file at: {1}.
        /// </summary>
        internal static string FeedNotInCache {
            get {
                return ResourceManager.GetString("FeedNotInCache", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find file or directory &apos;{0}&apos;..
        /// </summary>
        internal static string FileOrDirNotFound {
            get {
                return ResourceManager.GetString("FileOrDirNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The files you want to deleted are currently in use..
        /// </summary>
        internal static string FilesInUse {
            get {
                return ResourceManager.GetString("FilesInUse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to close these applications and continue?.
        /// </summary>
        internal static string FilesInUseAskClose {
            get {
                return ResourceManager.GetString("FilesInUseAskClose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following applications need to be closed:.
        /// </summary>
        internal static string FilesInUseInform {
            get {
                return ResourceManager.GetString("FilesInUseInform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finding duplicate files in &apos;{0}&apos;.
        /// </summary>
        internal static string FindingDuplicateFiles {
            get {
                return ResourceManager.GetString("FindingDuplicateFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file &apos;{0}&apos; is neither a symbolic link nor a regular file..
        /// </summary>
        internal static string IllegalFileType {
            get {
                return ResourceManager.GetString("IllegalFileType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The implementation &apos;{0}&apos; is already in the store..
        /// </summary>
        internal static string ImplementationAlreadyInStore {
            get {
                return ResourceManager.GetString("ImplementationAlreadyInStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The implementation &apos;{0}&apos; is damaged..
        /// </summary>
        internal static string ImplementationDamaged {
            get {
                return ResourceManager.GetString("ImplementationDamaged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to remove it? A new copy will be automatically downloaded again when needed..
        /// </summary>
        internal static string ImplementationDamagedAskRemove {
            get {
                return ResourceManager.GetString("ImplementationDamagedAskRemove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Run &apos;0install store remove {0}&apos; to remove it. A new copy will be automatically downloaded again when needed..
        /// </summary>
        internal static string ImplementationDamagedBatchInformation {
            get {
                return ResourceManager.GetString("ImplementationDamagedBatchInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The implementation &apos;{0}&apos; could not be located..
        /// </summary>
        internal static string ImplementationNotFound {
            get {
                return ResourceManager.GetString("ImplementationNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The underlying file system does not store file change times with sufficient accuracy.
        ///If you are using a FAT-formatted drive please convert it to NTFS..
        /// </summary>
        internal static string InsufficientFSTimeAccuracy {
            get {
                return ResourceManager.GetString("InsufficientFSTimeAccuracy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The archive contains an invalid path (potentially a security risk): {0}.
        /// </summary>
        internal static string InvalidPath {
            get {
                return ResourceManager.GetString("InvalidPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The manifest file contains invalid lines..
        /// </summary>
        internal static string InvalidLinesInManifest {
            get {
                return ResourceManager.GetString("InvalidLinesInManifest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid number of parts in line..
        /// </summary>
        internal static string InvalidNumberOfLineParts {
            get {
                return ResourceManager.GetString("InvalidNumberOfLineParts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The signature is invalid or damaged..
        /// </summary>
        internal static string InvalidSignature {
            get {
                return ResourceManager.GetString("InvalidSignature", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only administrators can optimise a shared store..
        /// </summary>
        internal static string MustBeAdminToOptimise {
            get {
                return ResourceManager.GetString("MustBeAdminToOptimise", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only administrators can remove elements from a shared store..
        /// </summary>
        internal static string MustBeAdminToRemove {
            get {
                return ResourceManager.GetString("MustBeAdminToRemove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file or directory name must not contain a newline character..
        /// </summary>
        internal static string NewlineInName {
            get {
                return ResourceManager.GetString("NewlineInName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No known digest method was specified..
        /// </summary>
        internal static string NoKnownDigestMethod {
            get {
                return ResourceManager.GetString("NoKnownDigestMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path &apos;{0}&apos; is not an absolute path. Relative paths are only allowed in portable mode.
        ///Specified in the configuration file &apos;{1}&apos;..
        /// </summary>
        internal static string NonRootedPathInConfig {
            get {
                return ResourceManager.GetString("NonRootedPathInConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No selected version.
        /// </summary>
        internal static string NoSelectedVersion {
            get {
                return ResourceManager.GetString("NoSelectedVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zero Install cannot remove itself from an implementation cache..
        /// </summary>
        internal static string NoStoreSelfRemove {
            get {
                return ResourceManager.GetString("NoStoreSelfRemove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (not cached).
        /// </summary>
        internal static string NotCached {
            get {
                return ResourceManager.GetString("NotCached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number value is too large..
        /// </summary>
        internal static string NumberTooLarge {
            get {
                return ResourceManager.GetString("NumberTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This option is controlled by a group policy and can therefore not be modified..
        /// </summary>
        internal static string OptionLockedByPolicy {
            get {
                return ResourceManager.GetString("OptionLockedByPolicy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem accessing the implementation store &apos;{0}&apos;..
        /// </summary>
        internal static string ProblemAccessingStore {
            get {
                return ResourceManager.GetString("ProblemAccessingStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem accessing the implementation store &apos;{0}&apos;.
        ///Specified in the configuration file &apos;{1}&apos;..
        /// </summary>
        internal static string ProblemAccessingStoreEx {
            get {
                return ResourceManager.GetString("ProblemAccessingStoreEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem loading a configuration file.
        ///You can delete the file &apos;{0}&apos; to fix the problem..
        /// </summary>
        internal static string ProblemLoadingConfigFile {
            get {
                return ResourceManager.GetString("ProblemLoadingConfigFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem loading the configuration value &apos;{0}&apos;.
        ///You can delete the file &apos;{1}&apos; to fix the problem. Other settings may also be lost..
        /// </summary>
        internal static string ProblemLoadingConfigValue {
            get {
                return ResourceManager.GetString("ProblemLoadingConfigValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reading files.
        /// </summary>
        internal static string ProcessingFiles {
            get {
                return ResourceManager.GetString("ProcessingFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Signature file already exists..
        /// </summary>
        internal static string SignatureAlreadyExists {
            get {
                return ResourceManager.GetString("SignatureAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to establish connection with StoreService..
        /// </summary>
        internal static string StoreServiceCommunicationProblem {
            get {
                return ResourceManager.GetString("StoreServiceCommunicationProblem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Temporary directories.
        /// </summary>
        internal static string TemporaryDirectories {
            get {
                return ResourceManager.GetString("TemporaryDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find a secret key with the specified name..
        /// </summary>
        internal static string UnableToFindSecretKey {
            get {
                return ResourceManager.GetString("UnableToFindSecretKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to enable write protection for &apos;{0}&apos;..
        /// </summary>
        internal static string UnableToWriteProtect {
            get {
                return ResourceManager.GetString("UnableToWriteProtect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown interface..
        /// </summary>
        internal static string UnknownInterface {
            get {
                return ResourceManager.GetString("UnknownInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The passphrase you entered is incorrect..
        /// </summary>
        internal static string WrongPassphrase {
            get {
                return ResourceManager.GetString("WrongPassphrase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The last line of the file is not the end of the signature block..
        /// </summary>
        internal static string XmlSignatureInvalidEnd {
            get {
                return ResourceManager.GetString("XmlSignatureInvalidEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The XML signature does not start in a new line..
        /// </summary>
        internal static string XmlSignatureMissingNewLine {
            get {
                return ResourceManager.GetString("XmlSignatureMissingNewLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The signature is not valid base 64:.
        /// </summary>
        internal static string XmlSignatureNotBase64 {
            get {
                return ResourceManager.GetString("XmlSignatureNotBase64", resourceCulture);
            }
        }
    }
}
