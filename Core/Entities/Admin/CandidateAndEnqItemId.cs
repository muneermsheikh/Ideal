using System;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
    public class CandidateAndEnqItemId      //: IComparable<CandidateAndEnqItemId>
    {
        public CandidateAndEnqItemId()
        {
        }

      //  public CandidateAndEnqItemId(int candidateID, int enquiryItemId)
//        {
    //        this.candidateID = candidateID;
  //          this.enquiryItemId = enquiryItemId;
      //  }

        public int candidateID {get; set;}
        public int enquiryItemId {get; set;}
        public int enquiryId {get; set; }
       /*
        public int CompareTo(CandidateAndEnqItemId c)
        {
            return this.enquiryItemId.CompareTo(c.enquiryItemId);
        }
        */

        /*
        of type: List(int, List<int>)
        var result = solvedExercises
            .GroupBy(e => e.StudentId)
            .ToDictionary(e => e.Key, e => e.Select(e2 => e2.ExerciseId).ToList());
        */
    }