using UnityEngine;
using System.Collections;

public class RunonSentencesScript : MonoBehaviour {

	public string[][] runonArray = new string[][]
	{
		//runonArray[0][i] -- Sentences that can be fixed by adding the conjunction “and”
		new string[] { 
			"An individual should evaluate all sources determine credibility.",
			"Critically evaluating sources strengthens research skills the final paper."
		},	

		//runonArray[1][i] -- Sentence can be fixed by creating a compound sentence. Add a comma and a conjunction (“, and”)
		new string[] {	
			"A thesis states the argument of a paper information in a paper should support the thesis."
		},

		//runonArray[2][i] -- Sentences that can be fixed by creating a new sentence. (add a “.” and capital letter)
		new string[] {
			"An individual should cite all information from sources plagiarism is illegal.",
			"Information can be located in a variety of ways use your school’s library resource center.",
			"Information comes in many forms you will want to figure out what purpose information serves when deciding whether or not to use it in your research.",
			"Timeliness of information can be determined by when it was published sometimes your topic requires current information.",
			"Forming research skills is a useful asset it will help an individual create a strong final paper or project.",
			"A writer can never do too much research sometimes it is a mistake to try to put all of the research that you find into your work you must know when to self edit.",
			"Information that you use in your paper should be relevant to your topic and answer a question you have posed make sure to look at a variety of different sources before deciding which one you will use.",
			"You can get a lot of information about an article by reading the citation you will want to read the article abstract to get an idea of the main points of the article.",
			"Familiarity with a topic can only be achieved through research demonstrate that you know what you are talking about with research."
		},

		//runonArray[3][i] -- Sentences that can be corrected by starting a new sentence and creating a compound sentence.
		new string[] {
			"Almost all sources have some degree of bias some sources will be fact some will be opinion."
		},

		//runonArray[4][i] -- Sentences that can be corrected by starting a new sentence and by using a comma to separate the independent clause
		new string[] {
			"When evaluating information be aware of where it comes from you want to be able to verify the information in another source it should be supported by evidence.",
			"When you look for information you will find a lot of it you need to be able to determine how useful it is for your purpose and to evaluate what you find.",
			"It’s a good idea to double check any citations that you copy and paste from a library database to check for accuracy it’s best to refer to the appropriate style guide."
		},

		//runonArray[5][i] -- Sentences that can be corrected by starting a new sentence and by using commas in a series. (commas in a series = ,,,,)
		new string[] {
			"One source of credibility you might look for are author credentials author credentials are things like educational background experience in the field and other writings on a similar topic.",
			"When looking at your citation, you will pay attention to things like author credentials currency of the topic and whether or not the article is peer reviewed make sure to skim the article after reading the abstract.",
			"Some essential research requirements include evaluating information thinking critically accrediting sources."
		},

		//runonArray[6][i] -- Correct sentence to see if students are paying attention
		new string[] {
			"Plagiarism occurs when an individual uses someone else's original ideas as his or her own."
		}
	};



	public string[][] correctArray = new string[][] 
	{
		//correctArray[0][i] -- Sentences that can be fixed by adding the conjunction “and”
		new string[] { 
			"An individual should evaluate all sources and determine credibility.",
			"Critically evaluating sources strengthens research skills and the final paper."
		},

		//correctArray[1][i] -- Sentence can be fixed by creating a compound sentence. Add a comma and a conjunction (“, and”)
		new string[] {
			"A thesis states the argument of a paper, and information in a paper should support the thesis."
		},

		//correctArray[2][i] -- Sentences that can be fixed by creating a new sentence. (add a “.” and capital letter)
		new string[] {
			"An individual should cite all information from sources. Plagiarism is illegal.",
			"Information can be located a variety of ways. Use your school’s library resource center.",
			"Information comes in many forms. You will want to figure out what purpose information serves when deciding whether or not to use it in your research.",
			"Timeliness of information can be determined by when it was published. Sometimes your topic requires current information.",
			"Forming research skills is a useful asset. It will help an individual create a strong final paper or project.",
			"A writer can never do too much research. Sometimes it is a mistake to try to put all of the research that you find into your work. You must know when to self edit.",
			"Information that you use in your paper should be relevant to your topic and answer a question you have posed. Make sure to look at a variety of different sources before deciding which one you will use.",
			"You can get a lot of information about an article by reading the citation. You will want to read the article abstract to get an idea of the main points of the article.",
			"Familiarity with a topic can only be achieved through research. Demonstrate that you know what you are talking about with research."
		},

		//correctArray[3][i] -- Sentences that can be corrected by starting a new sentence and creating a compound sentence.
		new string[]{
			"Almost all sources have some degree of bias. Some sources will be fact, and some will be opinion."
		},

		//correctArray[4][i] -- Sentences that can be corrected by starting a new sentence and by using a comma to separate the independent clause
		new string[]{
			"When evaluating information, be aware of where it comes from. You want to be able to verify the information in another source. It should be supported by evidence.",
			"When you look for information, you will find a lot of it. You need to be able to determine how useful it is for your purpose and to evaluate what you find.",
			"It’s a good idea to double check any citations that you copy and paste from a library database. To check for accuracy, it’s best to refer to the appropriate style guide."
		},

		//correctArray[5][i] -- Sentences that can be corrected by starting a new sentence and by using commas in a series. (commas in a series = ,,,,)
		new string[] {
			"One source of credibility you might look for are author credentials. Author credentials are things like educational background, experience in the field, and other writings on a similar topic.",
			"When looking at your citation, you will pay attention to things like author credentials, currency of the topic, and whether or not the article is peer reviewed. Make sure to skim the article after reading the abstract.",
			"Some essential research requirements include evaluating information, thinking critically, and accrediting sources."
		},

		//correctArray[6][i] -- Correct sentence to see if students are paying attention
		new string[] {
			"Plagiarism occurs when an individual uses someone else's original ideas as his or her own."
		}
	};

	public string[] hintArray = new string[]
	{
		"*This run-on can be fixed by adding a conjunction.",
		"*This run-on can be fixed by creating a compound sentence.",
		"*This run-on can be fixed by creating a new sentence.",
		"*This run-on can be corrected by starting a new sentence, and creating a compound sentence.",
		"*This run-on can be corrected by starting a new sentence, and by using a comma to separate the independent clause",
		"*This run-on can be corrected by starting a new sentence, and by using commas in a series.",
		"*There might be nothing wrong with this one..."
	};

}