using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LinguaDecks.Api.Models
{
	public class TeacherRequestsResponse
	{
		public IList<TeacherRequestModel> TeacherRequests { get; set; }=new List<TeacherRequestModel>();
		public int TotalTeacherRequests { get; set; } 
	}

}
