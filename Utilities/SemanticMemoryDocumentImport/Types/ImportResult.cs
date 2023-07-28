namespace SemanticMemoryDocumentImport;

public struct ImportResult{
   /// <summary>
        /// A boolean indicating whether the import is successful.
        /// </summary>
        public bool IsSuccessful => this.Keys.Any();

        /// <summary>
        /// The name of the collection that the document is inserted to.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// The keys of the inserted document chunks.
        /// </summary>
        public IEnumerable<string> Keys { get; set; } = new List<string>();

        /// <summary>
        /// Create a new instance of the <see cref="ImportResult"/> class.
        /// </summary>
        /// <param name="collectionName">The name of the collection that the document is inserted to.</param>
        public ImportResult(string collectionName)
        {
            this.CollectionName = collectionName;
        }

        /// <summary>
        /// Create a new instance of the <see cref="ImportResult"/> class representing a failed import.
        /// </summary>
        public static ImportResult Fail() => new(string.Empty);

        /// <summary>
        /// Add a key to the list of keys.
        /// </summary>
        /// <param name="key">The key to be added.</param>
        public void AddKey(string key)
        {
            this.Keys = this.Keys.Append(key);
        }
}