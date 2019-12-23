import React, { Component } from 'react';
import axios from 'axios';
import Table, { Thead, Tbody, Tr, Th, Td } from "react-row-select-table";
import { Form, Label, Input, Button, Text } from 'reactstrap';

var renderProducts = (products) => {
    return (
        <Table onCheck={value => console.log(value)} defaultCheckeds={[1,3]}>
            <Thead>
                <Tr>
                    <Th>Name</Th>
                    <Th>Quantity</Th>
                    <Th>Price</Th>
                </Tr>
            </Thead>
            <Tbody>
                {products.map(prod =>
                    <Tr key={prod["name"]}>
                        <Td>{prod["name"]}</Td>
                        <Td>{prod["quantity"]}</Td>
                        <Td>{(prod["dprice"])}</Td>
                    </Tr>
                )}
            </Tbody>
        </Table>
    );
}

export class ProductView extends Component {
    static diplayName = ProductView.name;

    constructor(props) {
        super(props);
        this.state = ({
            products: [],
            key: ""
      });
    }

    componentDidMount() {
        this.getProducts();
    }

    search = async() => {
        let form = new FormData();
        form.append('key', this.state.key);

        await axios.post('search', form)
            .then((result) => {
                this.setState({
                    products: result
                });
            })
            .catch((ex) => {
                console.error(ex);
            });
    }

    getKey(key) {
        this.setState({
            key: key.target.value,
        });
    }

    render() {
        let render = renderProducts(this.state.products);
        return (
            <div>
                <h1>Product Search</h1>
                <Form>
                    <h3>Products</h3>
                    <Input type="text" onChange={this.getKey.bind(this)} />
                    <Button type="submit" onClick={this.search}>Search</Button>
                </Form>
                {render}
            </div>
            );
    }

    async getProducts() {
        const response = await fetch('getProducts');
        const data = await response.json();
        this.setState({
            products: data
        });
    }
}