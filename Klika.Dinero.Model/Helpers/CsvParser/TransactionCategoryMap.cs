using System;
using System.Collections.Generic;
using System.Linq;
using Klika.Dinero.Model.Constants.Csv;
using Klika.Dinero.Model.Errors;

namespace Klika.Dinero.Model.Helpers.CsvParser
{   
    /// <summary>
    /// Supporting data class used to wrap database category to a category model optimized for keyword searching.
    /// Id transform into Key, name stays the same while Keywords get transformed into HashSet of strings.
    /// </summary>
    public class Category
    {
        public int Key { get; }
        public string Name { get; }
        public IEnumerable<string> Keywords { get; }

        public Category(int key, string name, string keywords)
        {
            Key = key;
            Name = name.ToLower();
            Keywords = keywords.Split(CsvConstants.KeywordsSeparator);
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is Category category)
            {
                return this.Key == category.Key;
            }

            return false;
        }
     
        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Name, Keywords);
        }
    }
    
    public sealed class InvalidCategoryConfiguration : Exception
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public InvalidCategoryConfiguration(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
    
    /// <summary>
    /// This class is a dictionary-like substituion to dictionary for storing transaction categories.
    /// It uses adapted Acho-Corasick string search algorithm that's used to search all (in this case first) occurance of the given keyword.
    /// Also, it initializes other and income category ID to reduce number of calls to the database.
    /// </summary>
    public class TransactionCategoryMap
    {
        private readonly AhoCorasick.Trie<int> Trie;
        public int? OtherCategoryId { get; set; }
        public int? IncomeCategoryId { get; set; }
        
        public TransactionCategoryMap(List<Category> categories)
        {
            Trie = new AhoCorasick.Trie<int>();

            foreach (var category in categories)
                foreach (var keyword in category.Keywords)
                    Trie.Add(keyword, category.Key);

            Trie.Build();
            
            OtherCategoryId = categories.FirstOrDefault(x => x.Name == CsvConstants.OtherCategoryName)?.Key;
            IncomeCategoryId = categories.FirstOrDefault(x => x.Name == CsvConstants.IncomeCategoryName)?.Key;

            if (OtherCategoryId is null)
                throw new InvalidCategoryConfiguration(ErrorCodes.InvalidConfiguration, ErrorDescriptions.InvalidOtherCategory);

            if (IncomeCategoryId is null)
                throw new InvalidCategoryConfiguration(ErrorCodes.InvalidConfiguration, ErrorDescriptions.InvalidIncomeCategory);
        }

        public int FindCategoryKey(string designation)
        {
            var keywords = Trie.FindAll(designation.ToLower()).ToList();
            
            return keywords.Count != 0 ? keywords.Last() : OtherCategoryId.Value;
        }
    }
}