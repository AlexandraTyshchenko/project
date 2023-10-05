import { useLocation } from 'react-router-dom';
import DeckInfoForm from '../../components/deckInfoForm/DeckInfoForm';
import './DeckForms.css';
import axios from 'axios';

function CreateDeck() {
  const location = useLocation();
  const user = location.state.user;
  const deck = location.state.deck;

  async function onSubmitHandler(values) {
    const api_URL = process.env.REACT_APP_API_URL;
    var request = {
      name: values.name,
      primaryLanguage: values.primaryLanguage,
      secondaryLanguage: values.secondaryLanguage,
      categoryId: parseInt(values.categoryId),
      authorId: user.id,
      description: values.description,
      icon: values.icon instanceof File ? values.icon : null
    };
    try {
      const response = await axios.post(`${api_URL}/api/Decks`, request, { headers: { 'Content-Type': 'multipart/form-data' }});
      window.location.assign(`decks/${response.data.id}`);
    }
    catch (error) {
      console.error('Error creating deck: ', error);
    }
  };

  return (
    <div id="content" class="container bg-primary-subtle bg-gradient">
      <div class='d-flex justify-content-center mt-3'>
        <h2 class='mb-0'>Create deck</h2>
      </div>
      <DeckInfoForm deck={deck} onSubmitHandler={onSubmitHandler}/>
    </div>
  );
};

export default CreateDeck;