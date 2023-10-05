import axios from 'axios';

const api_URL = process.env.REACT_APP_API_URL;

const SearchService = {

  async getAllDecks(
    currentPage,
    pageSize,
    selectedPrimaryLanguage,
    selectedSecondaryLanguage,
    _categoryId,
    author,
    title
  ) {
    try {
      const url = `${api_URL}/api/Decks?page=${currentPage}&pageSize=${pageSize}&Name=${title}&PrimaryLanguage=${selectedPrimaryLanguage}&SecondaryLanguage=${selectedSecondaryLanguage}&CategoryId=${_categoryId}&Author=${author}` 
      const response = await axios.get(url);
      console.log(response.data);
      if (response.status === 200) {

        return {
          deckitems: response.data.deckItems,
          totaldecks: response.data.totalDecks
        };
      }

      else {
        throw new Error(`error ${response.status}`);
      }
    } catch (error) {
      throw error;
    }
  },

  async getAllUsers(
    currentPage,
    pageSize,
    name,
    mail,
    role,
  ) {
    try {    
      var url = `${api_URL}/api/users?page=${currentPage}&pageSize=${pageSize}`;
      if(mail){
        url = url+`&Email=${mail}`;
      }
      if(name){
        url = url+`&Name=${name}`;
      }
      if(role !== undefined){
        url = url+`&Role=${role}`;
      }
            
      const response = await axios.get(url);
      if (response.status === 200) {
        return {
          userItems: response.data.userItems,
          totalUsers: response.data.totalUsers
        };
      }

      else {
        throw new Error(`error ${response.status}`);
      }
    } catch (error) {
      throw error;
    }
  }

};
export default SearchService;
