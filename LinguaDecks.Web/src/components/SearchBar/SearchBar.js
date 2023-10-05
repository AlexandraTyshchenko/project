import React, { useState, useEffect } from 'react';
import { Button, Dropdown } from 'react-bootstrap';
import languages from './languages';
import "./Searchbar.css"
import CategoryService from '../../services/CategoryService';

function SearchBar({ search }) {

    const [selectedPrimaryLanguage, setSelectedPrimaryLanguage] = useState("select language");
    const [selectedSecondaryLanguage, setSelectedSecondaryLanguage] = useState("select language");
    const [selectedCategory, setSelectedCategory] = useState("select category");
    const [author, setAuthor] = useState("");
    const [title, setTitle] = useState("");
    const [categories, SetCategories] = useState([]);
    const [categoryId, setCategoryId] = useState(0);

    function handleCategory(category) {
        setCategoryId(category.id);
        setSelectedCategory(category.name);
    }

    useEffect(() => {
        async function fetchData() {
            try {
                const responseData = await CategoryService.getAll();
                SetCategories(responseData);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        }
        fetchData();
    }, []);

    function handleClick() {
        search(selectedPrimaryLanguage, selectedSecondaryLanguage, categoryId, author, title);
    }

    function Reset() {
        setSelectedPrimaryLanguage("select language");
        setSelectedSecondaryLanguage("select language");
        setSelectedCategory("select category");
        setAuthor("");
        setTitle("");
        setCategoryId(0);
        search("", "", 0, "", "");
    }

    return (
        <div className='container cont'>
            <div className='row'>
                <div className='col-6 d-flex justify-content-center'>
                    <input className="input form-control " type="text" placeholder="title" value={title} onChange={(event) => setTitle(event.target.value)} />
                </div>
                <div className='col-6 d-flex justify-content-center'>
                    <input className="input form-control" type="text" placeholder="author" value={author} onChange={(event) => setAuthor(event.target.value)} />
                    <Button className='mybtn' onClick={() => handleClick()}>Search</Button>
                    <Button className='mybtn btn-light' onClick={() => Reset()}>Reset</Button>
                </div>
            </div>
            <div className='row'>
                <div className='col-12'>
                    <div className='container'>
                        <div className='row' style={{ padding: '10px' }}>
                            <div className='col-4 d-flex justify-content-center'>
                                <div>
                                    <h6 className="text-center">Primary language</h6>
                                    <Dropdown>
                                        <Dropdown.Toggle variant="light" id="language-dropdown">
                                            {selectedPrimaryLanguage}
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu>
                                            {languages.map((language) => (
                                                <Dropdown.Item
                                                    onClick={() => setSelectedPrimaryLanguage(language.name, language.id)}
                                                    key={language.id}
                                                >
                                                    {language.name}
                                                </Dropdown.Item>
                                            ))}
                                        </Dropdown.Menu>
                                    </Dropdown>
                                </div>
                            </div>
                            <div className='col-4 d-flex justify-content-center'>
                                <div>
                                    <h6 className="text-center">Secondary language</h6>
                                    <Dropdown>
                                        <Dropdown.Toggle variant="light" id="language-dropdown">
                                            {selectedSecondaryLanguage}
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu>
                                            {languages.map((language) => (
                                                <Dropdown.Item
                                                    onClick={() => setSelectedSecondaryLanguage(language.name)}
                                                    key={language.id}
                                                >
                                                    {language.name}
                                                </Dropdown.Item>
                                            ))}
                                        </Dropdown.Menu>
                                    </Dropdown>
                                </div>
                            </div>

                            <div className='col-4 d-flex justify-content-center'>
                                <div>
                                    <h6 className="text-center">Category</h6>
                                    <Dropdown>
                                        <Dropdown.Toggle variant="light" id="category-dropdown">
                                            {selectedCategory}
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu>
                                            {categories.map((category) => (
                                                <Dropdown.Item
                                                    onClick={() => handleCategory(category)}
                                                    key={category.id}
                                                >
                                                    {category.name}
                                                </Dropdown.Item>
                                            ))}
                                        </Dropdown.Menu>
                                    </Dropdown>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default SearchBar;