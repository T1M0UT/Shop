var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        productModel: {
            id: 0,
            name: "Product Name",
            description: "Product Description",
            price: 99.99
        },
        products: [],
    },
    mounted(){
        this.getProducts();
    },
    methods: {
        resetModel(){
            this.productModel = {
                id: 0,
                    name: "Product Name",
                    description: "Product Description",
                    price: 99.99
            }
        },
        getProduct(id){
            this.loading = true;
            axios.get('/Admin/products/' + id)
                .then(res => {
                    console.log(res.data)
                    const product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        price: product.price,
                    };
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getProducts(){
            this.loading = true;
            axios.get('/Admin/products')
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
        createProduct(){
            this.loading = true;
            axios.post('/Admin/products', this.productModel)
                .then(res => {
                    console.log(res.data)
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        updateProduct(){
            this.loading = true;
            console.log(this.productModel);

            axios.put('/Admin/products', this.productModel)
                .then(res => {
                    console.log(res.data)
                    this.products.splice(this.objectIndex, 1, res.data);
                })
                .catch(err => {
                    console.log(this.productModel);
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        deleteProduct(id, index){
            this.loading = true;
            axios.delete('/Admin/products/' + id)
                .then(res => {
                    console.log(res)
                    this.products.splice(index, 1)
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newProduct(){
            this.editing = true;
            this.productModel.id = 0;
        },
        editProduct(id, index){
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },
        cancel() {
            this.editing = false;
        }
    },
    computed: {
        
    }
});