import React, { useEffect, useState } from 'react';
import axios from 'axios';
import DeckInfo from './ViewDeck/ViewDeck';
import defaultUserLogo from '../assets/default_user_avatar.png';
import deckIcon from '../assets/default_deck_logo.svg';
import Loading from './Loading';
import { useParams } from 'react-router-dom';
import { getAuth } from "../services/auth";
import Comment from '../components/comment/Comment';

function DeckPage() { 
  //maybe the next line should be in another place 
  const { id } = useParams();
  const api_URL = process.env.REACT_APP_API_URL;
  const user = getAuth()?.user;
	const [deck, setDeck] = useState(null);
  const [progress, setProgress] = useState(null);
  const [deckRating, setDeckRating] = useState(null);
  const [comments, setComments] = useState(null);
  const [update,Update]=useState(true);
  const token = getAuth()?.accessToken;
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  
	useEffect(() => {
    function isValidUrl(string) {
      if (!string) {
        return false;
      }
      if (string === "") {
        return false;
      }
      try {
        new URL(string);
        return true;
      } catch (_) {
        return false;
      }
    }
    
    function validateCommentData(comment){
      if(!isValidUrl(comment.user.iconPath)){
        comment.user.iconPath = defaultUserLogo;
      }
      const date = new Date(comment.date);
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      comment.date = `${day}.${month}.${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
    }
    
    function validateComments(comments){
      comments.forEach(comment => {
        validateCommentData(comment)
      });
    }
    
    function validateDeckData(deck){
      if(!isValidUrl(deck.iconPath)){ 
        deck.iconPath = deckIcon;
      }
    }

    

		async function fetchData () {
			await new Promise(resolve => setTimeout(resolve, 500));
      try {
				const response = await axios.get(`${api_URL}/api/Decks/${id}`);
        validateDeckData(response.data.deck);
        validateComments(response.data.comments);
       
				setDeck(response.data.deck);
      	setDeckRating(response.data.deckRating);
      	setComments(response.data.comments);
        
        if(user){
          const response2 = await axios.get(`${api_URL}/api/Decks/${id}/progress`);
          setProgress(response2.data);
        }
        
			} 
			catch (error) {
				console.error('Error fetching data: ', error);
			}
      
		};
    
    fetchData();
    }, [id, api_URL, update]);
    function SetUpdate() {
        Update(!update);
    }
    if (!deck || !deckRating || !comments || (user) ? !progress : false) {
    return (
      <Loading />
    );
  }

	return(    
 
 <DeckInfo
			Deck={deck}
			DeckRating={deckRating}
			User={user}
			Comments={comments}
      UserProgress = {progress}
      SetUpdate={SetUpdate}
		/>	
   

   
	);
}
export default DeckPage;