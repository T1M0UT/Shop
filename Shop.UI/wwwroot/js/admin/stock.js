var app = new Vue({
    el: '#app',
    data:{
        products: [],
        selectedProduct: null,
        loading: false,
        newStock: {
            productId: 0,
            description: "Size",
            quantity: 10,
        }
    },
    mounted() {
        this.getStock();
    },
    methods: {
        getStock(){
            this.loading = true;
            axios.get('/stocks')
                .then(res => {
                    console.log(res.data)
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        addStock() {
            this.loading = true;
            axios.post('/stocks', this.newStock)
                .then(res => {
                    console.log(res.data)
                    this.selectedProduct.stock.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateStock() {
            this.loading = true;
            axios.put('/stocks', {
                stock: this.selectedProduct.stock.map(x => {
                    return {
                        id: x.id,
                        productId: this.selectedProduct.id,
                        description: x.description,
                        quantity: x.quantity
                    }
                })
            })
                .then(res => {
                    console.log(res.data)
                    this.selectedProduct.stock.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        deleteStock(id, index) {
            this.loading = true;
            axios.delete('/stocks/' + id)
                .then(res => {
                    console.log(res.data)
                    this.selectedProduct.stock.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        selectProduct(product){
            this.selectedProduct = product;
            this.newStock.productId = product.id;
        }
    }
})