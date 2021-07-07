const webpack = require('webpack'); // eslint-disable-line import/no-extraneous-dependencies

module.exports = {
    configureWebpack: {
        plugins: [
            new webpack.IgnorePlugin({
                resourceRegExp: /^\.\/locale$/,
                contextRegExp: /moment$/,
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery',
                'window.jQuery': 'jquery',
                Popper: ['popper.js', 'default'],
            }),
        ],
        optimization: {
            splitChunks: {
                cacheGroups: {
                    commons: {
                        name: 'commons',
                        priority: -20,
                        chunks: 'initial',
                        minChunks: 2,
                        reuseExistingChunk: true,
                        enforce: true,
                    },
                },
            },
        },
    },
    chainWebpack: (config) => {
        config.optimization
            .delete('splitChunks');
        // raw-loader
        config.module
            .rule('aspx')
            .test(/\.aspx$/)
            .use('raw-loader')
            .loader('raw-loader')
            .end()
    },
    publicPath: './',
    pages: {
        rates_admin: {
            entry: 'src/views/rates-admin/main.js',
            template: 'public/rates-admin.aspx',
            filename: 'rates-admin.aspx',
            chunks: ['commons', 'rates_admin'],
        },
        reservation_list: {
            entry: 'src/views/reservation-list/main.js',
            template: 'public/reservation-list.aspx',
            filename: 'reservation-list.aspx',
            chunks: ['commons', 'reservation_list'],
        },
        reservation_details: {
            entry: 'src/views/reservation-details/main.js',
            template: 'public/reservation-details.aspx',
            filename: 'reservation-details.aspx',
            chunks: ['commons', 'reservation_details'],
        },
        hotel_config: {
            entry: 'src/views/hotel-config/main.js',
            template: 'public/hotel-config.aspx',
            filename: 'hotel-config.aspx',
            chunks: ['commons', 'hotel_config'],

        },
        mapping_f2g: {
            entry: 'src/views/mapping-f2g/main.js',
            template: 'public/mapping-rate-plans.aspx',
            filename: 'mapping-rate-plans.aspx',
            chunks: ['commons', 'mapping_f2g'],
        },
        promotions:{
            entry: 'src/views/promotions/main.js',
            template: 'public/promotions.aspx',
            filename: 'promotions.aspx',
            chunks: ['commons', 'promotions'] 
        },
        promotions_details: {
            entry: 'src/views/promotions-details/main.js',
            template: 'public/promotions-details.aspx',
            filename: 'promotions-details.aspx',
            chunks: ['commons', 'promotions_details']
        },
        rooms_closure:{
            entry:'src/views/rooms-closure/main.js',
            template:'public/rooms-closure.aspx',
            filename:'rooms-closure.aspx',
            chunks:['commons','rooms_closure']
        }
        /* hotel_list: {
            entry: 'src/views/hotel-list/main.js',
            template: 'public/hotel-list.aspx',
            filename: 'hotel-list.aspx',
        }, */
        // subpage: 'src/subpage/main.js'
    },
};