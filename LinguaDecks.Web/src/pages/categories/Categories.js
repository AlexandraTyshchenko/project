import React, { useEffect, useState } from "react";
import CategoryService from "../../services/CategoryService";
import axios from "axios";
import './Categories.css';
import DeleteConfirmationModal from "../../components/admin-modal-forms/DeleteConfirmationModal";
import { getAuth } from "../../services/auth";
function Categories() {
  const token = getAuth().accessToken;
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  const api_URL = process.env.REACT_APP_API_URL;
  const [categories, setCategories] = useState([]);
  const [newCategoryName, setNewCategoryName] = useState("");
  const [deleteCategoryId, setDeleteCategoryId] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsLoading(true);
        const response = await CategoryService.getAll();
        setCategories(response);
        setIsLoading(false);
        setError("");
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    }

    fetchData();
  }, []);

  async function handleAddCategory() {
    try {
      await axios.post(
        `${api_URL}/api/Categories?categoryName=${newCategoryName}`,
        { categoryName: newCategoryName }
      );
      const response = await CategoryService.getAll();
      setCategories(response);
      setError("");
    } catch (error) {
      if (error.response && error.response.data && error.response.data.Details) {
        setError(error.response.data.Details);
      } else {
        setError(error.message);
      }
    }
    setNewCategoryName("");
  }

  function showDeleteConfirmationModal(categoryId) {
    setDeleteCategoryId(categoryId);
    setShowDeleteModal(true);
  }

  async function handleDeleteCategory() {
    try {
      await axios.delete(
        `${api_URL}/api/Categories?id=${deleteCategoryId}`
      );
      
      const response = await CategoryService.getAll();
      setCategories(response);
      setError("");
    } catch (error) {
      setError(error.message);
    }
    setShowDeleteModal(false);
    setDeleteCategoryId(null);
  }

  return (
    <div>
      {error ? (
        <div className="alert alert-danger text-center" role="alert">
          {error}
        </div>
      ) : null}
      {isLoading ? (
        <div className="d-flex justify-content-center " style={{ height: '100vh' }}>
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      ) : (
        <div>
          <input
            type="text"
            placeholder="New Category Name"
            value={newCategoryName}
            onChange={(e) => setNewCategoryName(e.target.value)}
          />
          <div className="btn btn-primary ms-1" onClick={handleAddCategory}>Add Category</div>
        </div>
      )}
      <table className="table table-striped">
        <thead>
          <tr>
            <th>Category Name</th>
            <th>Delete</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((category) => (
            <tr key={category.id}>
              <td>{category.name}</td>
              <td>
                <button type="button" className="btn btn-danger position-relative" onClick={() => showDeleteConfirmationModal(category.id)}>
                  <i className="fas fa-shopping-cart position-absolute top-0 start-0 translate-middle"></i>
                  <span className="bin" >🗑</span>
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <DeleteConfirmationModal
        show={showDeleteModal}
        onHide={() => setShowDeleteModal(false)}
        onConfirmDelete={handleDeleteCategory}
      />
    </div>
  );
}

export default Categories;
