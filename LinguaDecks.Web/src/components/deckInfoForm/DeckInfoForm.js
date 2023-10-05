import { Field, Form, Formik, ErrorMessage } from 'formik';
import * as Yup from "yup";
import './DeckInfoForm.css';
import { useNavigate } from 'react-router-dom';
import languages from '../../components/SearchBar/languages';
import CategoryService from '../../services/CategoryService';
import { useEffect, useState } from 'react';
import deckIcon from '../../assets/default_deck_logo.svg'

function DeckInfoForm(props) {
  const deck = props.deck;
  const sortedLanguages = languages.sort((a, b) => {
    const a1 = a.name.toLowerCase();
    const b1 = b.name.toLowerCase();

    return (a1 > b1) ? 1 : (a1 < b1) ? -1 : 0;
  });

  const navigate = useNavigate();

  const [categories, setCategories] = useState([]);
  const [previewImage, setPreviewImage] = useState(deck?.iconPath ?? deckIcon);

  useEffect(() => {
    async function fetchData() {
      try {
        const categories = await CategoryService.getAll();
        categories.sort((a, b) => {
          const a1 = a.name.toLowerCase();
          const b1 = b.name.toLowerCase();

          return (a1 > b1) ? 1 : (a1 < b1) ? -1 : 0;
        });
        setCategories(categories);
      } catch (error) {
        console.error('Error fetching data: ', error);
      }
    }
    fetchData();
  }, []);

  const handleImagePreview = (e) => {
    const file = e.currentTarget.files[0];

    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewImage(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const signInSchema = Yup.object().shape({
    name: Yup.string()
      .trim("Name can't have trailing whitespaces")
      .strict(true)
      .required("Name is required")
      .max(50, "Name is too long"),
    primaryLanguage: Yup.string()
      .required("Original language is required"),
    secondaryLanguage: Yup.string()
      .required("Translation language is required"),
    categoryId: Yup.number()
      .test("value", "Category is required", (value) => value > 0),
    description: Yup.string()
      .trim("Description can't have trailing whitespaces")
      .strict(true)
      .notRequired()
      .max(1000, "Description is too long"),
    icon: Yup.mixed()
      .notRequired()
      .test("fileSize", "File size must be less than 500 KB", (value) => {
        if (value && value.size && value !== deckIcon) {
          return value.size <= 500 * 1024; // 500 KB
        }
        return true;
      })
  });

  const initialValues = {
    name: deck?.name ?? '',
    primaryLanguage: deck?.primaryLanguage ?? '',
    secondaryLanguage: deck?.secondaryLanguage ?? '',
    categoryId: deck?.category?.id ?? 0,
    description: deck?.description ?? '',
    icon: deck?.iconPath ?? deckIcon
  };

  return (
    <div id="deckDisplay" class="row justify-content-center">
      <div id="deckInfo" class="col-md-6 mt-3 ">
        <Formik
          initialValues={initialValues}
          validationSchema={signInSchema}
          onSubmit={(values) => {
            props.onSubmitHandler(values);
          }}>
          {(formik) => {
            const { errors, touched, isValid, dirty, values } = formik
            return (
              <div>
                <Form>
                  {previewImage && (
                    <div id="deck-icon" class="bg-light border border-primary-subtle border-3 rounded">
                      <img src={previewImage} alt="Deck Icon" class="mx-auto d-block" />
                    </div>
                  )}
                  <div class='row mt-4 mb-2'>
                    <label htmlFor='icon' class='col-sm-4 col-form-label'>Image</label>
                    <div class='col-sm-8'>
                      <input
                        id='icon'
                        type='file'
                        name='icon'
                        accept='image/*'
                        class={'form-control'.concat(errors.icon ? ' input-error' : '')}
                        onChange={(e) => {
                          formik.setFieldValue('icon', e.currentTarget.files[0]);
                          handleImagePreview(e);
                        }}
                      />
                      <span class='d-block mt-1 error'>{errors.icon}</span>
                    </div>
                  </div>

                  <div class='row mb-2'>
                    <label for='name' class='col-sm-4 col-form-label'>Name</label>
                    <div class='col-sm-8'>
                      <Field
                        id='name'
                        type='text'
                        name='name'
                        class={'form-control'.concat(errors.name && touched.name ? ' input-error' : '')}
                        placeholder='Name' />
                      <ErrorMessage name='name' component='span' class='error' />
                    </div>
                  </div>
                  <div class='row mb-2'>
                    <label for='primaryLanguage' class='col-sm-4 col-form-label'>Original language</label>
                    <div class='col-sm-8'>
                      <Field id='primaryLanguage' as='select' name='primaryLanguage' class='form-select'>
                        {!values.primaryLanguage && (<option value=''>Choose language</option>)}
                        {sortedLanguages.map((language) => (
                          <option key={language.id} value={language.name}>
                            {language.name}
                          </option>
                        ))}
                      </Field>
                      <ErrorMessage name='primaryLanguage' component='span' class='error' />
                    </div>
                  </div>
                  <div class='row mb-2'>
                    <label for='secondaryLanguage' class='col-sm-4 col-form-label'>Translation language</label>
                    <div class='col-sm-8'>
                      <Field id='secondaryLanguage' as='select' name='secondaryLanguage' class='form-select'>
                        {!values.secondaryLanguage && (<option value=''>Choose language</option>)}
                        {sortedLanguages.map((language) => (
                          <option key={language.id} value={language.name}>
                            {language.name}
                          </option>
                        ))}
                      </Field>
                      <ErrorMessage name='secondaryLanguage' component='span' class='error' />
                    </div>
                  </div>
                  <div class='row mb-2'>
                    <label for='categoryId' class='col-sm-4 col-form-label'>Category</label>
                    <div class='col-sm-8'>
                      <Field id='categoryId' as='select' name='categoryId' class='form-select'>
                        {values.categoryId === 0 && (<option value=''>Choose category</option>)}
                        {categories.map((category) => (
                          <option key={category.id} value={category.id}>
                            {category.name}
                          </option>
                        ))}
                      </Field>
                      <ErrorMessage name='categoryId' component='span' class='error' />
                    </div>
                  </div>
                  <div class='row mb-2'>
                    <label for='description' class='col-sm-4 col-form-label'>Description</label>
                    <div class='col-sm-8 mb-2'>
                      <Field
                        id='description'
                        as='textarea'
                        rows='4'
                        name='description'
                        class={'form-control'.concat(errors.description && touched.description ? ' input-error' : '')}
                        placeholder='Description' />
                      <ErrorMessage name='description' component='span' class='error' />
                    </div>
                  </div>
                  <div class='row'>
                    <button type='submit' class='col border btn bg-primary text-white' disabled={!(dirty && isValid)}>Save</button>
                    <button type='button' class='col border btn bg-primary text-white' onClick={() => navigate(-1)}>Cancel</button>
                  </div>
                </Form>
              </div>
            );
          }}
        </Formik>
      </div>
    </div>
  );
};

export default DeckInfoForm;