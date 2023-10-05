import React, { useState, useEffect } from 'react';
import DeckItem from '../components/DeckItem/DeckItem';
import SearchService from '../services/SearchService';
import { Link } from 'react-router-dom';
import Pagination from '../components/Pagination/Pagination';
import SearchBar from '../components/SearchBar/SearchBar';
import { getAuth } from '../services/auth';
import './DeckList.css';

function DeckList() {
  const pageSize = 10;
  const [currentPage, setCurrentPage] = useState(1);
  const [deckitems, setDeckItems] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedPrimaryLanguage, setSelectedPrimaryLanguage] = useState("");
  const [selectedSecondaryLanguage, setSelectedSecondaryLanguage] = useState("");
  const [author, setAuthor] = useState("");
  const [title, setTitle] = useState("");
  const [error, setError] = useState(null);
  const [categoryId, setCategoryId] = useState(0);
  const [user, setUser] = useState(null);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsLoading(true);
        const responseData = await SearchService.getAllDecks(
          currentPage,
          pageSize,
          selectedPrimaryLanguage,
          selectedSecondaryLanguage,
          categoryId,
          author,
          title
        );
        const authResponse = await getAuth();
        setTotalPages(Math.ceil(responseData.totaldecks / pageSize));
        setDeckItems(responseData.deckitems);
        setUser(authResponse?.user);
      } catch (error) {
        setError(error.message); 
        console.error('Error fetching data:', error);
      } finally {
        setIsLoading(false);
      }
    }
    fetchData();
  }, [currentPage, selectedPrimaryLanguage, selectedSecondaryLanguage, categoryId, author, title]);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  const search = (selectedPrimaryLanguage, selectedSecondaryLanguage, categoryId, author, title) => {
    if (selectedPrimaryLanguage !== 'select language') {
      setSelectedPrimaryLanguage(selectedPrimaryLanguage);
    }
    if (selectedSecondaryLanguage !== 'select language') {
      setSelectedSecondaryLanguage(selectedSecondaryLanguage);
    }
    setCategoryId(categoryId);
    setAuthor(author);
    setTitle(title);
    setCurrentPage(1);
  };

  return (
    <div>
      <SearchBar search={search} />
      <div class={'d-flex justify-content-'.concat(user?.role === 'Teacher' || user?.role === 'Admin' ? 'between' : 'center')}>
        <div id='paginationCenterer' class={user?.role !== 'Teacher' && user?.role !== 'Admin' ? 'd-none' : ''}></div>
        <Pagination paginate={paginate} totalPages={totalPages} currentPage={currentPage} />
        <Link
          to={'/decks'}
          state={{user: user}}
          class={user?.role === 'Teacher' || user?.role === 'Admin' ? 'me-4' : 'd-none'}
          style={{marginTop: '10px'}}>
          <div class = "btn btn-primary">Create deck</div>
        </Link>
      </div>

      {isLoading ? (
        <div className="d-flex justify-content-center " style={{ height: '100vh' }}>
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      ) : error ? (
        <div className="d-flex justify-content-center">
          {error}
        </div>
      ) : deckitems.length > 0 ? (
        deckitems.map((deck) => (
          <Link to={`/decks/${deck.id}`} key={deck.id}>
            <DeckItem
              key={deck.id}
              title={deck.name}
              cardsnumber={deck.cardsCount}
              category={deck.category}
              rate={deck.rating}
              author={deck.author}
              id={deck.id}
            />
          </Link>
        ))
      ) : (
        <div className="d-flex justify-content-center ">
          <p>Nothing found</p>
        </div>
      )}
    </div>
  );
}

export default DeckList;
