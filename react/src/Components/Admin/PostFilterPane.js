import { useState, useEffect } from "react";
import Form from 'react-bootstrap/Form';
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import { getFilter } from "../../Services/BlogRepository";
import { FormGroup } from "react-bootstrap";
import { useSelector, useDispatch } from 'react/redux';
import {
    reset,
    updateAuthorId,
    updateCategoryId,
    updateKeyword,
    updateMonth,
    updateYear
} from '../../Redux/Reducer';


const PostFilterPane = () => {
    const postFilter = useSelector(state => state.postFilter),
        dispatch = useDispatch(),
        [filter, setFilter] = useState({
            authorList: [],
            categoryList: [],
            monthList: []
        });
    const handleReset = (e) => {
        dispatch(reset());
    };
    useEffect(() => {
        getFilter().then(data => {
            if (data) {
                setFilter({
                    authorList: data.authorList,
                    categoryList: data.categoryList,
                    monthList: data.monthList
                });
            } else {
                setFilte(initialState);
            }
        })
    }, [])
    return (
        <Form method="get"
            onSubmit={handleReset}
            className="row gy-2 gx-3 align-items-center p-2">
            <FormGroup className="col-auto">
                <Form.Label className="visually-hidden">
                    keyword
                </Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Nhap tu khoa..."
                    name="keyword"
                    value={postFilter.keyword}
                    onChange={e => dispatch(updateKeyword(e.target.value))} />
            </FormGroup>
            <FormGroup className="col-auto">
                <Form.Label className="visually-hidden">
                    authorId
                </Form.Label>
                <Form.Select name='authorId'
                    value={postFilter.authorId}
                    onChange={e => dispatch(updateAuthorId(e.target.value))}
                    title="Author Id"
                >
                    <option value=''>--Chon Tac Gia--</option>
                    {filter.authorList.length > 0 &&
                        filter.authorList.map((item, index) =>
                            <option key={index} value={item.value}>{item.text}</option>
                        )}
                </Form.Select>
            </FormGroup>
            <FormGroup className="col-auto">
                <Form.Label className="visually-hidden">
                    categoryId
                </Form.Label>
                <Form.Select name='categoryId'
                    value={postFilter.catgoryId}
                    onChange={e => dispatch(updateCategoryId(e.target.value))}
                    title="Category Id"
                >
                    <option value=''>--Chon Chu De--</option>
                    {filter.categoryList.length > 0 &&
                        filter.categoryList.map((item, index) =>
                            <option key={index} value={item.value}>{item.text}</option>
                        )}
                </Form.Select>
            </FormGroup>
            <FormGroup className="col-auto">
                <Form.Label className="visually-hidden">
                    year
                </Form.Label>
                <Form.Control
                    type="number"
                    placeholder="Nhap nam..."
                    name="year"
                    value={postFilter.year}
                    max={postFilter.year}
                    onChange={e => dispatch(updateYear(e.target.value))} />
            </FormGroup>
            <FormGroup className="col-auto">
                <Form.Label className="visually-hidden">
                    month
                </Form.Label>
                <Form.Select
                    value={postFilter.month}
                    onChange={e => dispatch(updateMonth(e.target.value))}
                    title="month">
                    <option value=''>--Chon Thang--</option>
                    {filter.monthList.length > 0 &&
                        filter.monthList.map((item, index) =>
                            <option key={index} value={item.value}>{item.text}</option>
                        )}
                </Form.Select>
            </FormGroup>
            <Form.Group className="col-auto">
                <Button variant='primary' type='reset'>
                    Xoa/Loc
                </Button>
                <Link to='/admin/posts/edit' className='btn btn-success ms-2'>Them moi</Link>
            </Form.Group>
        </Form>
    );
}

export default PostFilterPane;