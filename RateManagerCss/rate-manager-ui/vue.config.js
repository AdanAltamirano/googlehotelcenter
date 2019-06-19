const webpack = require('webpack')

module.exports = {
    configureWebpack: {
        plugins: [
          new webpack.IgnorePlugin({
            resourceRegExp: /^\.\/locale$/,
            contextRegExp: /moment$/
          })
        ]
    },
    chainWebpack: config => {
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
        },
        reservation_list:
        {
          entry: 'src/views/reservation-list/main.js',
          template: 'public/reservation-list.aspx',
          filename: 'reservation-list.aspx'
        }
        // subpage: 'src/subpage/main.js'
    },
};
