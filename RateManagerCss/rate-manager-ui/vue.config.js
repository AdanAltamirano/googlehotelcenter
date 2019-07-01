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
    },
    chainWebpack: (config) => {
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
        },
        reservation_list: {
            entry: 'src/views/reservation-list/main.js',
            template: 'public/reservation-list.aspx',
            filename: 'reservation-list.aspx',
        },
        /*hotel_list: {
            entry: 'src/views/hotel-list/main.js',
            template: 'public/hotel-list.aspx',
            filename: 'hotel-list.aspx',
        },*/
        // subpage: 'src/subpage/main.js'
    },
};
