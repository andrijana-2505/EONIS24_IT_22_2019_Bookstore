import axios from 'axios';
import React, {useState} from 'react';

import useCategories from '../../hooks/useCategories';
import useAuth from '../../hooks/useAuth';
import {CategoryCreateDto} from '../../dto/category/CategoryCreateDto';
import {CategoryUpdateDto} from '../../dto/category/CategoryUpdateDto';

const CategoryOperations = () => {
    const [selectedCategory, setSelectedCategory] = useState<number>(0);
    const [newCategoryName, setNewCategoryName] = useState<string>('');
    const [updatedCategoryName, setUpdatedCategoryName] = useState<string>('');

    const {categories} = useCategories();
    const {userToken} = useAuth();

    const createCategory = async () => {
        try {
            const categoryCreateDto: CategoryCreateDto = {
                categoryName: newCategoryName,
            };
            await axios.post(
                'http://localhost:5137/api/Category',
                categoryCreateDto,
                {
                    headers: {
                        Authorization: `Bearer ${userToken?.token}`,
                    },
                }
            );
            setNewCategoryName('');
            alert('Category created');
        } catch (error) {
            console.error(error);
        }
    };

    const updateCategory = async () => {
        try {
            const categoryUpdateDto: CategoryUpdateDto = {
                categoryId: selectedCategory,
                categoryName: updatedCategoryName,
            };
            await axios.put('http://localhost:5137/api/Category', categoryUpdateDto, {
                headers: {
                    Authorization: `Bearer ${userToken?.token}`,
                },
            });
            setUpdatedCategoryName('');
            alert('Category updated');
        } catch (error) {
            console.error(error);
        }
    };

    const deleteCategory = async () => {
        try {
            await axios.delete(
                `http://localhost:5137/api/Category/${selectedCategory}`,
                {
                    headers: {
                        Authorization: `Bearer ${userToken?.token}`,
                    },
                }
            );
            alert('Category deleted');
        } catch (error) {
            console.error(error);
        }
    };

    const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedCategory(Number(e.target.value));
    };

    const handleCreateSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        createCategory();
    };

    const handleUpdateSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        updateCategory();
    };

    const handleDeleteSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        deleteCategory();
    };

    return (
        <div className='admin-operations'>
            <h1>Category operations</h1>
            <div className='admin-operations__form-container'>
                <h3>Create new category</h3>
                <form
                    className='admin-operations__form-container__form'
                    onSubmit={handleCreateSubmit}
                >
                    <input
                        type='text'
                        name=''
                        id=''
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                            setNewCategoryName(e.target.value)
                        }
                        placeholder='Enter category name ...'
                        required={true}
                    />
                    <button>Create Category</button>
                </form>
            </div>
            <div className='admin-operations__form-container'>
                <h3>Update category</h3>

                <form
                    className='admin-operations__form-container__form'
                    onSubmit={handleUpdateSubmit}
                >
                    <select id='category' name='category' onChange={handleSelectChange} required={true}>
                        <option>Select...</option>
                        {categories && categories.length > 0 ? (
                            categories.map((category) => (
                                <option key={category.categoryId} value={category.categoryId}>
                                    {category.categoryName}
                                </option>
                            ))
                        ) : (
                            <option value='' disabled>
                                No categories available
                            </option>
                        )}
                    </select>
                    <input
                        type='text'
                        name=''
                        id=''
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                            setUpdatedCategoryName(e.target.value)
                        }
                        required={true}
                        placeholder='Enter a new name for existing category ...'
                    />
                    <button disabled={!categories || categories.length === 0}>
                        Update Category
                    </button>
                </form>
            </div>
            <div className='admin-operations__form-container'>
                <h3>Delete category</h3>

                <form
                    className='admin-operations__form-container__form'
                    onSubmit={handleDeleteSubmit}
                >
                    <select id='category' name='category' onChange={handleSelectChange} required={true}>
                        <option>Select...</option>
                        {categories && categories.length > 0 ? (
                            categories.map((category) => (
                                <option key={category.categoryId} value={category.categoryId}>
                                    {category.categoryName}
                                </option>
                            ))
                        ) : (
                            <option value='' disabled>
                                No categories available
                            </option>
                        )}
                    </select>
                    <button disabled={!categories || categories.length === 0}>
                        Delete Category
                    </button>
                </form>
            </div>
        </div>
    );
};

export default CategoryOperations;
