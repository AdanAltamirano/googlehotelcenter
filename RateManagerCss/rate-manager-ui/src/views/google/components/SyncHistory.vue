<template>
  <div>
    <!-- Filtros -->
    <b-row class="mt-3 mb-2">
      <b-col md="3">
        <b-form-group :label="$t('Status')">
          <b-form-select v-model="filterStatus" :options="statusOptions" @change="resetAndLoad"></b-form-select>
        </b-form-group>
      </b-col>
      <b-col md="4">
        <b-form-group :label="$t('Operation')">
          <b-form-select v-model="filterOperation" :options="operationOptions" @change="resetAndLoad"></b-form-select>
        </b-form-group>
      </b-col>
      <b-col md="2" class="d-flex align-items-end pb-3">
        <b-button variant="outline-primary" @click="resetAndLoad" :disabled="loading">
          <i class="fa fa-refresh"></i> {{ $t('Refresh') }}
        </b-button>
      </b-col>
    </b-row>

    <!-- Tarjetas resumen -->
    <b-row class="mb-3" v-if="!loading && total > 0">
      <b-col sm="6" md="3" class="mb-2">
        <div class="sync-stat sync-stat--total">
          <div class="sync-stat__number">{{ total }}</div>
          <div class="sync-stat__label">TOTAL</div>
        </div>
      </b-col>
      <b-col sm="6" md="3" class="mb-2">
        <div class="sync-stat sync-stat--success">
          <div class="sync-stat__number">{{ successCount }}</div>
          <div class="sync-stat__label">SUCCESS</div>
        </div>
      </b-col>
      <b-col sm="6" md="3" class="mb-2">
        <div class="sync-stat sync-stat--failed">
          <div class="sync-stat__number">{{ failedCount }}</div>
          <div class="sync-stat__label">FAILED</div>
        </div>
      </b-col>
      <b-col sm="6" md="3" class="mb-2">
        <div class="sync-stat sync-stat--pending">
          <div class="sync-stat__number">{{ pendingCount }}</div>
          <div class="sync-stat__label">PENDING</div>
        </div>
      </b-col>
    </b-row>

    <!-- Spinner -->
    <div v-if="loading" class="vld-parent mt-4" style="height:100px;">
      <loading :active="true" :is-full-page="false" color="#007bff"></loading>
    </div>

    <!-- Tabla -->
    <div v-else>
      <b-table
        :items="items"
        :fields="tableFields"
        striped hover small responsive show-empty
        :empty-text="$t('No records found')">

        <template #cell(status)="{ value }">
          <b-badge :variant="statusVariant(value)">{{ value }}</b-badge>
        </template>

        <template #cell(timestamp)="{ value }">
          {{ formatDate(value) }}
        </template>

        <template #cell(errorMessage)="{ value }">
          <span v-if="value" class="text-danger small" :title="value">{{ truncate(value, 80) }}</span>
        </template>

        <template #cell(actions)="{ item }">
          <b-button size="sm" variant="outline-secondary" @click="openXml(item)" title="Ver XML">
            <i class="fa fa-code"></i>
          </b-button>
        </template>
      </b-table>

      <b-pagination
        v-if="total > pageSize"
        v-model="currentPage"
        :total-rows="total"
        :per-page="pageSize"
        @change="onPageChange"
        align="center"
        class="mt-2">
      </b-pagination>
    </div>

    <!-- ===== OVERLAY PROPIO (Vue 2, position:fixed sobre el viewport) ===== -->
    <transition name="overlay-fade">
      <div v-if="xmlModalVisible && selectedItem"
           class="sh-overlay"
           @click.self="closeModal">

        <div class="sh-popup">

          <!-- Header -->
          <div class="sh-header">
            <div class="sh-header-left">
              <span class="sh-id">#{{ selectedItem.idSync }}</span>
              <span class="sh-sep">—</span>
              <span class="sh-op">{{ selectedItem.tipoOperacion }}</span>
              <span class="sh-badge" :class="'sh-badge--' + statusVariant(selectedItem.status)">
                {{ selectedItem.status }}
              </span>
              <span v-if="selectedItem.ratePlanId" class="sh-plan">
                <i class="fa fa-tag"></i> {{ selectedItem.ratePlanId }}
              </span>
            </div>
            <div class="sh-header-right">
              <span class="sh-date"><i class="fa fa-clock-o"></i> {{ formatDate(selectedItem.timestamp) }}</span>
              <button class="sh-close" @click="closeModal"><i class="fa fa-times"></i></button>
            </div>
          </div>

          <!-- Subheader -->
          <div v-if="selectedItem.usuario || selectedItem.errorMessage" class="sh-sub">
            <span v-if="selectedItem.usuario" class="sh-user">
              <i class="fa fa-user"></i> {{ selectedItem.usuario }}
            </span>
            <div v-if="selectedItem.errorMessage" class="sh-error">
              <i class="fa fa-exclamation-circle"></i>
              <strong>Error:</strong> {{ selectedItem.errorMessage }}
            </div>
          </div>

          <!-- Tabs -->
          <div class="sh-tabs">
            <button class="sh-tab" :class="{ active: activeTab === 0 }" @click="activeTab = 0">
              <i class="fa fa-upload"></i> Request XML
            </button>
            <button class="sh-tab" :class="{ active: activeTab === 1 }" @click="activeTab = 1">
              <i class="fa fa-download"></i> Response XML
            </button>
          </div>

          <!-- XML body -->
          <div class="sh-body">
            <div class="sh-toolbar">
              <span class="sh-toolbar-label"><i class="fa fa-code"></i> {{ activeTab === 0 ? 'Request' : 'Response' }}</span>
              <button class="sh-copy" @click="copyToClipboard(activeTab === 0 ? selectedItem.requestXML : selectedItem.responseXML)">
                <i class="fa fa-copy"></i> Copiar
              </button>
            </div>
            <pre class="sh-xml">{{ activeTab === 0 ? formatXml(selectedItem.requestXML) : formatXml(selectedItem.responseXML) }}</pre>
          </div>

        </div>
      </div>
    </transition>

    <!-- Toast -->
    <transition name="toast-fade">
      <div v-if="copied" class="sh-toast">
        <i class="fa fa-check-circle"></i> Copiado al portapapeles
      </div>
    </transition>

  </div>
</template>

<script>
import Loading from 'vue-loading-overlay';
import ConfluxService from '../../../api/conflux-service';

export default {
  components: { Loading },
  props: { hotelId: {} },
  data() {
    return {
      loading: false,
      items: [], total: 0,
      successCount: 0, failedCount: 0, pendingCount: 0,
      currentPage: 1, pageSize: 50,
      filterStatus: '', filterOperation: '',
      xmlModalVisible: false,
      selectedItem: null,
      activeTab: 0,
      copied: false,
      tableFields: [
        { key: 'idSync',        label: '#',          thStyle: { width: '65px'  } },
        { key: 'timestamp',     label: 'Fecha/Hora', thStyle: { width: '150px' } },
        { key: 'tipoOperacion', label: 'Operación'  },
        { key: 'ratePlanId',    label: 'Plan'        },
        { key: 'usuario',       label: 'Usuario'     },
        { key: 'status',        label: 'Estado',     thStyle: { width: '100px' } },
        { key: 'errorMessage',  label: 'Error'       },
        { key: 'actions',       label: '',           thStyle: { width: '50px'  } }
      ]
    };
  },
  computed: {
    statusOptions() {
      return [
        { value: '', text: this.$t('All') },
        { value: 'Success', text: 'Success' },
        { value: 'Failed',  text: 'Failed'  },
        { value: 'Pending', text: 'Pending' }
      ];
    },
    operationOptions() {
      return [
        { value: '',                       text: this.$t('All')           },
        { value: 'UpdateRate',             text: 'UpdateRate'             },
        { value: 'UpdateRatePatch',        text: 'UpdateRatePatch'        },
        { value: 'DeleteRate',             text: 'DeleteRate'             },
        { value: 'DeleteRatePlan',         text: 'DeleteRatePlan'         },
        { value: 'InsertRatePlan',         text: 'InsertRatePlan'         },
        { value: 'UpdateRestriction',      text: 'UpdateRestriction'      },
        { value: 'UpdateRestrictionPatch', text: 'UpdateRestrictionPatch' },
        { value: 'ActivateRatePlan',       text: 'ActivateRatePlan'       },
        { value: 'DeactivateRatePlan',     text: 'DeactivateRatePlan'     }
      ];
    }
  },
  created() { this.loadHistory(); },
  mounted()  { window.addEventListener('keydown', this.onKey); },
  beforeDestroy() { window.removeEventListener('keydown', this.onKey); },
  methods: {
    onKey(e) { if (e.key === 'Escape') this.closeModal(); },
    closeModal() {
      this.xmlModalVisible = false;
      document.body.style.overflow = '';
    },
    openXml(item) {
      this.selectedItem    = item;
      this.activeTab       = 0;
      this.xmlModalVisible = true;
      document.body.style.overflow = 'hidden'; // evita scroll detrás del overlay
    },
    resetAndLoad() { this.currentPage = 1; this.loadHistory(); },
    loadHistory() {
      this.loading = true;
      ConfluxService.GetSyncHistory(this.hotelId, {
        status:        this.filterStatus    || undefined,
        tipoOperacion: this.filterOperation || undefined,
        pageSize:      this.pageSize,
        page:          this.currentPage
      }).then(r => {
        const d = r.body;
        this.items        = d.items;
        this.total        = d.total;
        this.successCount = d.successCount;
        this.failedCount  = d.failedCount;
        this.pendingCount = d.pendingCount;
        this.loading = false;
      }).catch(() => { this.loading = false; });
    },
    onPageChange(p) { this.currentPage = p; this.loadHistory(); },
    statusVariant(s) {
      return { Success: 'success', Failed: 'danger', Pending: 'warning' }[s] || 'secondary';
    },
    formatDate(v) {
      if (!v) return '';
      const d = new Date(v);
      return isNaN(d) ? v : d.toLocaleString('es-MX');
    },
    truncate(s, max) {
      if (!s) return '';
      return s.length > max ? s.substring(0, max) + '…' : s;
    },
    formatXml(xml) {
      if (!xml) return '(vacío)';
      try {
        const PAD = '  '; let pad = 0;
        xml = xml.replace(/(>)(<)(\/*)/g, '$1\n$2$3');
        return xml.split('\n').map(line => {
          let indent = 0;
          if      (line.match(/.+<\/\w[^>]*>$/))     { indent = 0; }
          else if (line.match(/^<\/\w/) && pad > 0)   { pad--;      }
          else if (line.match(/^<\w[^>]*[^\/]>.*$/)) { indent = 1; }
          const r = PAD.repeat(pad) + line; pad += indent; return r;
        }).join('\n');
      } catch { return xml; }
    },
    copyToClipboard(text) {
      if (!text) return;
      navigator.clipboard.writeText(text).then(() => {
        this.copied = true;
        setTimeout(() => { this.copied = false; }, 2500);
      });
    }
  }
};
</script>

<style scoped>
/* ── Stat cards ───────────────────────────── */
.sync-stat { border-radius:10px; padding:14px 16px; text-align:center; color:#fff; box-shadow:0 2px 8px rgba(0,0,0,.15); }
.sync-stat__number { font-size:2rem; font-weight:700; line-height:1; }
.sync-stat__label  { font-size:.72rem; margin-top:6px; opacity:.85; letter-spacing:1px; }
.sync-stat--total   { background:linear-gradient(135deg,#6c757d,#495057); }
.sync-stat--success { background:linear-gradient(135deg,#28a745,#1e7e34); }
.sync-stat--failed  { background:linear-gradient(135deg,#dc3545,#b21f2d); }
.sync-stat--pending { background:linear-gradient(135deg,#ffc107,#e0a800); color:#333; }

/* ── Overlay ──────────────────────────────────────────────────────────────
   position:fixed se ancla al VIEWPORT, no a la página.
   No le importa cuánto haya scrolleado el usuario.
──────────────────────────────────────────────────────────────────────── */
.sh-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  z-index: 99999;
  background: rgba(0,0,0,.70);
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding: 8px 24px 24px;
  overflow-y: auto;
}

/* ── Popup box ────────────────────────────── */
.sh-popup {
  background: #1e1e1e;
  border-radius: 12px;
  box-shadow: 0 24px 64px rgba(0,0,0,.75);
  width: 100%;
  max-width: 1225px;
  max-height: 42vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

/* ── Header ───────────────────────────────── */
.sh-header {
  background: linear-gradient(135deg,#1a1a2e,#16213e);
  padding: 14px 20px;
  display: flex; align-items: center; justify-content: space-between;
  flex-wrap: wrap; gap: 8px;
  border-bottom: 1px solid rgba(255,255,255,.07);
  flex-shrink: 0;
}
.sh-header-left  { display:flex; align-items:center; gap:10px; flex-wrap:wrap; }
.sh-header-right { display:flex; align-items:center; gap:12px; }
.sh-id  { color:rgba(255,255,255,.45); font-size:.92rem; font-weight:700; }
.sh-sep { color:rgba(255,255,255,.25); }
.sh-op  { color:#fff; font-size:1rem; font-weight:600; }
.sh-badge { font-size:.73rem; font-weight:600; padding:3px 9px; border-radius:20px; color:#fff; }
.sh-badge--success   { background:#28a745; }
.sh-badge--danger    { background:#dc3545; }
.sh-badge--warning   { background:#ffc107; color:#333; }
.sh-badge--secondary { background:#6c757d; }
.sh-plan { background:rgba(255,255,255,.1); color:rgba(255,255,255,.82); font-size:.78rem; padding:2px 9px; border-radius:4px; }
.sh-date { color:rgba(255,255,255,.5); font-size:.8rem; white-space:nowrap; }
.sh-close {
  background:transparent; border:none; color:rgba(255,255,255,.55);
  font-size:1.1rem; cursor:pointer; padding:4px 8px; border-radius:4px;
  line-height:1; transition:background .15s,color .15s;
}
.sh-close:hover { background:rgba(255,255,255,.1); color:#fff; }

/* ── Sub-header ───────────────────────────── */
.sh-sub { background:#252526; padding:8px 20px; border-bottom:1px solid #333; flex-shrink:0; }
.sh-user { font-size:.8rem; color:#888; }
.sh-error {
  margin-top:6px; font-size:.8rem;
  background:rgba(220,53,69,.18); border:1px solid rgba(220,53,69,.38);
  border-radius:6px; padding:5px 10px; color:#ffb3ba;
}

/* ── Tabs ─────────────────────────────────── */
.sh-tabs { background:#252526; border-bottom:2px solid #333; display:flex; flex-shrink:0; }
.sh-tab {
  background:transparent; border:none; color:#777; font-size:.84rem;
  padding:10px 18px; cursor:pointer;
  border-bottom:2px solid transparent; margin-bottom:-2px;
  transition:color .15s,border-color .15s;
}
.sh-tab:hover { color:#bbb; }
.sh-tab.active { color:#4fc3f7; border-bottom-color:#4fc3f7; font-weight:600; }

/* ── XML body ─────────────────────────────── */
.sh-body { display:flex; flex-direction:column; flex:1; overflow:hidden; min-height:0; }
.sh-toolbar {
  background:#252526; padding:6px 14px;
  display:flex; align-items:center; justify-content:space-between;
  border-bottom:1px solid #333; flex-shrink:0;
}
.sh-toolbar-label { color:#555; font-size:.7rem; font-family:monospace; text-transform:uppercase; letter-spacing:1px; }
.sh-copy {
  background:transparent; border:1px solid #444; color:#999;
  font-size:.76rem; padding:3px 10px; border-radius:4px; cursor:pointer;
  transition:all .15s;
}
.sh-copy:hover { background:rgba(255,255,255,.07); color:#fff; border-color:#777; }
.sh-xml {
  flex:1; overflow:auto;
  background:#1e1e1e; color:#d4d4d4;
  font-size:.76rem; line-height:1.6;
  padding:16px 18px; margin:0; white-space:pre;
  font-family:'Courier New',Consolas,'Fira Code',monospace;
  max-height: 24vh;
}

/* ── Toast ────────────────────────────────── */
.sh-toast {
  position:fixed; bottom:28px; right:28px; z-index:999999;
  background:#28a745; color:#fff;
  padding:10px 20px; border-radius:8px;
  font-size:.88rem; box-shadow:0 4px 16px rgba(0,0,0,.35);
  pointer-events:none; display:flex; align-items:center; gap:6px;
}

/* ── Transiciones ─────────────────────────── */
.overlay-fade-enter-active,.overlay-fade-leave-active { transition:opacity .2s; }
.overlay-fade-enter,.overlay-fade-leave-to { opacity:0; }
.toast-fade-enter-active,.toast-fade-leave-active { transition:opacity .3s,transform .3s; }
.toast-fade-enter,.toast-fade-leave-to { opacity:0; transform:translateY(8px); }
</style>
