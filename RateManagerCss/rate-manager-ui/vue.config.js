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
                        enforce: true
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
            .end();
    },
    publicPath: './',
    pages: {
        rates_admin: {
            entry: 'src/views/rates-admin/main.js',
            template: 'public/rates-admin.aspx',
            filename: 'rates-admin.aspx',
            chunks: [ 'commons', 'rates_admin'],
        },
        reservation_list: {
            entry: 'src/views/reservation-list/main.js',
            template: 'public/reservation-list.aspx',
            filename: 'reservation-list.aspx',
            chunks: [ 'commons', 'reservation_list'],
        },
        /* hotel_list: {
            entry: 'src/views/hotel-list/main.js',
            template: 'public/hotel-list.aspx',
            filename: 'hotel-list.aspx',
        }, */
        // subpage: 'src/subpage/main.js'
    },
};
