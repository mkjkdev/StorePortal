import React, { Component } from 'react';
import axios from 'axios';
import { Form, Label, Input, Button, Text } from 'reactstrap';

export class ProductAdmin extends Component {
    static displayName = ProductAdmin.name;

    constructor(props) {
        super(props);
        this.state = ({
            name: "",
            price: "",
            quantity: ""
        });
    }

    getName(product) {
        this.setState({
            name : product.target.value
        });
    }
    getQuantity(product) {
        this.setState({
            quantity: product.target.value
        });
    }
    getPrice(product) {
        this.setState({
            price: product.target.value
        });
    }

    postProduct = async () => {
        let form = new FormData();

        form.append('name', this.state.name);
        form.append('price', this.state.price);
        form.append('quantity', this.state.quantity);

        await axios.post('createProducts', form)
            .then((result) => {
                let message = "Success!"
                result ? this.setState({ status: message, logged: true })
                    : this.setState({ status: "Fail" });
            })
            .catch((ex) => {
                console.error(ex);
            });
    }

    render() {
        return (
            <div>
                <h3></h3>
                <Form>
                    <Label for="prodName">Product Name</Label>
                    <Input type="text" onChange={this.getName.bind(this)} />
                    <Label for="prodQuantity">Quantity</Label>
                    <Input type="text" onChange={this.getQuantity.bind(this)} />
                    <Label for="prodPrice">Price</Label>
                        <Input type="text" onChange={this.getPrice.bind(this)} />
                    <Button type="submit" onClick={this.postProduct}>Add product</Button>
                    <p></p>
                </Form>
            </div>
            );
    }
}