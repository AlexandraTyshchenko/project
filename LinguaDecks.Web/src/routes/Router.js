import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from '../pages/Home';
import DeckPage from '../pages/DeckPage';
import DeckGame from '../pages/DeckGame';
import TeacherRequestPage from '../pages/TeacherRequestPage';
import Categories from '../pages/categories/Categories';
import Profile from '../pages/Profile';
import SignUp from '../pages/authForms/SignUp';
import SignIn from '../pages/authForms/SignIn';
import EditDeck from '../pages/deckForms/EditDeck';
import CreateDeck from '../pages/deckForms/CreateDeck';
import UserList from '../pages/UserListPage';
import Test from '../pages/ViewDeck/Test'
function AppRouter() {
  return (
    <Routes>
      <Route element={<Home />} path="/" />
      <Route element={<DeckPage />} path="/decks/:id" />
      <Route element={<DeckGame />} path="/decks/:id/cards" />
      <Route element={<SignIn />} path="/signin" />
      <Route element={<SignUp />} path="/signup" />
      <Route element={<TeacherRequestPage />} path="/teacherrequests" />
      <Route element={<Categories />} path="/categories" />
      <Route element={<Profile />} path="/profile" />
      <Route element={<EditDeck />} path="/decks/:id/edit" />
      <Route element={<CreateDeck />} path="/decks" />
      <Route element={<UserList/>} path="/users" />
      <Route element={<Test/>} path="/test" />
    </Routes>
  );
}

export default AppRouter;
