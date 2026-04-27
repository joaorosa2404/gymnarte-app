import { useState, useEffect } from 'react'

// ── Types ─────────────────────────────────────────────────────────────────────
interface Auth { token: string; userId: number; fullName: string; role: string }
interface Bio { w: string; h: string; f: string; l: string; wa: string; v: string; a: number; d: string }
interface User { id: number; i: string; c: string; name: string; un: string; email: string; ph: string; g: string; b: string; scope: string; cr: string; plans: number; bio: Bio | null }
interface Exercise { id: number; name: string; type: string; typeId: number; notes: string; inPlans: number }
interface ExType { id: number; name: string; desc: string; count: number; created: string; updated: string; updatedBy: string }
interface Scope { id: number; role: string; users: number; created: string; updated: string; updatedBy: string }

// ── Mock data ─────────────────────────────────────────────────────────────────
const MOCK_USERS: User[] = [
  { id:1001,i:'MC',c:'#f97316',name:'Miguel Costa',un:'miguel.costa',email:'miguel@email.com',ph:'912 345 678',g:'Masculino',b:'15/04/1996',scope:'Member',cr:'12/01/2024',plans:2,bio:{w:'82.5',h:'178',f:'18.2',l:'72.1',wa:'58.3',v:'9',a:28,d:'15/03/2025'}},
  { id:1002,i:'AS',c:'#6366f1',name:'Ana Silva',un:'ana.silva',email:'ana@email.com',ph:'961 234 567',g:'Feminino',b:'03/08/1993',scope:'Trainer',cr:'15/01/2024',plans:3,bio:{w:'61.3',h:'165',f:'22.4',l:'68.9',wa:'55.1',v:'5',a:31,d:'10/03/2025'}},
  { id:1003,i:'JR',c:'#10b981',name:'João Rosa',un:'joao.rosa',email:'joao@email.com',ph:'934 567 890',g:'Masculino',b:'22/11/1990',scope:'Admin',cr:'01/01/2024',plans:0,bio:null},
  { id:1004,i:'LP',c:'#8b5cf6',name:'Luísa Pinto',un:'luisa.pinto',email:'luisa@email.com',ph:'916 789 012',g:'Feminino',b:'07/02/2001',scope:'Member',cr:'20/03/2025',plans:1,bio:{w:'58.0',h:'162',f:'24.1',l:'65.2',wa:'53.8',v:'4',a:24,d:'20/03/2025'}},
  { id:1005,i:'RF',c:'#ec4899',name:'Rui Faria',un:'rui.faria',email:'rui@email.com',ph:'927 890 123',g:'Masculino',b:'18/06/1988',scope:'Member',cr:'03/02/2024',plans:1,bio:{w:'91.2',h:'182',f:'21.5',l:'69.0',wa:'56.4',v:'12',a:36,d:'05/03/2025'}},
]
const MOCK_EX: Exercise[] = [
  {id:1,name:'Agachamento',type:'Musculação',typeId:1,notes:'Foco em quadricípites',inPlans:3},
  {id:2,name:'Supino Plano',type:'Musculação',typeId:1,notes:'—',inPlans:5},
  {id:3,name:'Corrida',type:'Cardio',typeId:2,notes:'Velocidade variável',inPlans:2},
  {id:4,name:'Remada Baixa',type:'Musculação',typeId:1,notes:'—',inPlans:4},
  {id:5,name:'Prancha',type:'Flexibilidade',typeId:3,notes:'Core e estabilidade',inPlans:6},
  {id:6,name:'Puxada Frontal',type:'Musculação',typeId:1,notes:'Dorsal e bíceps',inPlans:3},
]
const MOCK_TYPES: ExType[] = [
  {id:1,name:'Musculação',desc:'Exercícios de força com pesos',count:58,created:'01/01/2024',updated:'—',updatedBy:'—'},
  {id:2,name:'Cardio',desc:'Exercícios aeróbicos de resistência',count:21,created:'01/01/2024',updated:'—',updatedBy:'—'},
  {id:3,name:'Flexibilidade',desc:'Alongamentos e mobilidade',count:10,created:'05/01/2024',updated:'10/03/2025',updatedBy:'JR'},
  {id:4,name:'Misto',desc:'Combinação de modalidades',count:5,created:'10/01/2024',updated:'—',updatedBy:'—'},
]
const MOCK_SCOPES: Scope[] = [
  {id:1,role:'Admin',users:1,created:'01/01/2024',updated:'—',updatedBy:'—'},
  {id:2,role:'Trainer',users:5,created:'01/01/2024',updated:'15/02/2025',updatedBy:'João Rosa'},
  {id:3,role:'Member',users:142,created:'01/01/2024',updated:'—',updatedBy:'—'},
]

// ── Helpers ───────────────────────────────────────────────────────────────────
function scopeBadge(s: string) {
  const cls = s === 'Admin' ? 'gbr' : s === 'Trainer' ? 'gba' : 'gbb'
  return <span className={`badge ${cls}`}>{s}</span>
}
function typeBadge(t: string) {
  const cls = t === 'Cardio' ? 'gba' : t === 'Flexibilidade' ? 'gbg' : 'gbb'
  return <span className={`badge ${cls}`}>{t}</span>
}
function initials(name: string) { return name.split(' ').map(p => p[0]).join('').slice(0,2).toUpperCase() }
function avatarColor(name: string) {
  const cols = ['#f97316','#6366f1','#10b981','#8b5cf6','#ec4899','#14b8a6','#f59e0b','#3b82f6']
  let h = 0; for (const c of name) h = (h * 31 + c.charCodeAt(0)) & 0xffffff
  return cols[Math.abs(h) % cols.length]
}

// ── Modal ─────────────────────────────────────────────────────────────────────
function Modal({ title, onClose, children }: { title: string; onClose: () => void; children: React.ReactNode }) {
  return (
    <div className="moverlay" onClick={e => { if (e.target === e.currentTarget) onClose() }}>
      <div className="mbox">
        <div className="mh">
          <span className="mtt">{title}</span>
          <button className="mcl" onClick={onClose}>×</button>
        </div>
        {children}
      </div>
    </div>
  )
}

// ── User Detail ───────────────────────────────────────────────────────────────
function UserDetail({ u, onClose }: { u: User; onClose: () => void }) {
  const [tab, setTab] = useState('ov')
  return (
    <div className="ud">
      <div className="ph">
        <div className="pa" style={{ background: u.c }}>{u.i}</div>
        <div style={{ flex: 1 }}>
          <div className="pn">{u.name}</div>
          <div className="psub">#{u.id} · {u.un} · {u.email}</div>
          <div className="pbdg">{scopeBadge(u.scope)}<span className="badge gbg">Ativo</span></div>
        </div>
        <div className="pstats">
          <div className="pst"><div className="psv">{u.plans}</div><div className="psl">Planos</div></div>
          <div className="pst"><div className="psv">{u.bio ? 'Sim' : '—'}</div><div className="psl">Biometria</div></div>
          <div className="pst"><div className="psv">2</div><div className="psl">Notif.</div></div>
        </div>
        <div className="pac">
          <button className="btn">Editar</button>
          <button className="btn" style={{ color: '#dc2626' }}>Desativar</button>
        </div>
      </div>

      <div className="tabs">
        {[['ov','Resumo'],['bio','Biometria'],['pl','Planos de treino'],['no','Notificações']].map(([id,lbl]) => (
          <div key={id} className={`tab${tab === id ? ' on' : ''}`} onClick={() => setTab(id)}>{lbl}</div>
        ))}
      </div>

      {tab === 'ov' && (
        <div className="row2">
          <div className="card">
            <div className="ch"><span className="ct">Dados pessoais</span></div>
            {[['Nome',u.name],['Username',u.un],['Email',u.email],['Telefone',u.ph],['Género',u.g],['Data nasc.',u.b],['Perfil',null],['Membro desde',u.cr]].map(([l,v]) => (
              <div key={l as string} className="ir">
                <span className="il">{l}</span>
                <span className="iv">{l === 'Perfil' ? scopeBadge(u.scope) : v}</span>
              </div>
            ))}
          </div>
          <div>
            <div className="card" style={{ marginBottom: 10 }}>
              <div className="ch"><span className="ct">Última medição</span>{u.bio && <span style={{ fontSize: 10, color: '#888' }}>{u.bio.d}</span>}</div>
              {u.bio ? (
                <>
                  <div className="ir"><span className="il">Peso</span><span className="iv">{u.bio.w} kg</span></div>
                  <div className="ir"><span className="il">Altura</span><span className="iv">{u.bio.h} cm</span></div>
                  <div className="ir"><span className="il">% Gordura</span><span className="iv">{u.bio.f}%</span></div>
                  <div className="ir"><span className="il">% Água</span><span className="iv">{u.bio.wa}%</span></div>
                  <div className="ir"><span className="il">Gordura visceral</span><span className="iv">{u.bio.v}</span></div>
                </>
              ) : <div style={{ padding: '12px', fontSize: 11, color: '#999' }}>Sem dados registados.</div>}
            </div>
            <div className="card">
              <div className="ch"><span className="ct">Notificações recentes</span></div>
              <div className="nr"><div className="nd u"></div><div><div style={{ fontSize: 11.5 }}>Plano de treino atualizado</div><div style={{ fontSize: 10, color: '#888' }}>há 1 hora</div></div></div>
              <div className="nr"><div className="nd u"></div><div><div style={{ fontSize: 11.5 }}>Novos dados biométricos</div><div style={{ fontSize: 10, color: '#888' }}>há 3 horas</div></div></div>
            </div>
          </div>
        </div>
      )}

      {tab === 'bio' && (
        <>
          {u.bio && (
            <div className="bg4">
              {[['Peso',u.bio.w,'kg'],['Altura',u.bio.h,'cm'],['% Gordura',u.bio.f,'%'],['% M. Magra',u.bio.l,'%']].map(([l,v,un]) => (
                <div key={l as string} className="bcard">
                  <div className="blbl">{l}</div>
                  <div className="bval">{v}<span className="bunt"> {un}</span></div>
                </div>
              ))}
            </div>
          )}
          <div className="card">
            <div className="ch"><span className="ct">Histórico biométrico</span><button className="btn btp">+ Novo registo</button></div>
            {u.bio ? (
              <table><thead><tr><th>Data</th><th>Peso</th><th>Altura</th><th>Idade</th><th>% Gordura</th><th>% M. Magra</th><th>% Água</th><th>G. Visceral</th><th>Estado</th><th></th></tr></thead>
                <tbody><tr>
                  <td>{u.bio.d}</td><td>{u.bio.w} kg</td><td>{u.bio.h} cm</td><td>{u.bio.a}</td>
                  <td>{u.bio.f}%</td><td>{u.bio.l}%</td><td>{u.bio.wa}%</td><td>{u.bio.v}</td>
                  <td><span className="badge gbg">Ativo</span></td>
                  <td><button className="ib">Editar</button></td>
                </tr></tbody>
              </table>
            ) : <div style={{ padding: 18, textAlign: 'center', fontSize: 11.5, color: '#999' }}>Sem dados biométricos.<br /><button className="btn btp" style={{ marginTop: 8 }}>+ Primeiro registo</button></div>}
          </div>
        </>
      )}

      {tab === 'pl' && (
        <div className="card">
          <div className="ch"><span className="ct">Planos de treino</span><button className="btn btp">+ Novo plano</button></div>
          {u.plans > 0 ? (
            <table><thead><tr><th>Nome</th><th>Tipo</th><th>Exercícios</th><th>Notas</th><th></th></tr></thead>
              <tbody>
                <tr><td><strong>Plano Força A</strong></td><td>{typeBadge('Musculação')}</td><td>6</td><td>Foco em pernas e costas</td><td><div style={{ display: 'flex', gap: 4 }}><button className="ib">Ver</button><button className="ib">Editar</button><button className="ib dl">Remover</button></div></td></tr>
                {u.plans > 1 && <tr><td><strong>Cardio Queima</strong></td><td>{typeBadge('Cardio')}</td><td>4</td><td>30 min por sessão</td><td><div style={{ display: 'flex', gap: 4 }}><button className="ib">Ver</button><button className="ib">Editar</button><button className="ib dl">Remover</button></div></td></tr>}
                {u.plans > 2 && <tr><td><strong>Full Body</strong></td><td>{typeBadge('Musculação')}</td><td>8</td><td>—</td><td><div style={{ display: 'flex', gap: 4 }}><button className="ib">Ver</button><button className="ib">Editar</button><button className="ib dl">Remover</button></div></td></tr>}
              </tbody>
            </table>
          ) : <div style={{ padding: 18, textAlign: 'center', fontSize: 11.5, color: '#999' }}>Sem planos atribuídos.</div>}
        </div>
      )}

      {tab === 'no' && (
        <div className="card">
          <div className="ch"><span className="ct">Notificações</span><button className="btn">Marcar todas como lidas</button></div>
          <div className="nr"><div className="nd u"></div><div style={{ flex: 1 }}><div style={{ fontSize: 11.5 }}>Plano de treino atualizado</div><div style={{ fontSize: 10, color: '#888' }}>há 1 hora · Não lida</div></div><button className="ib">Marcar lida</button></div>
          <div className="nr"><div className="nd u"></div><div style={{ flex: 1 }}><div style={{ fontSize: 11.5 }}>Novos dados biométricos</div><div style={{ fontSize: 10, color: '#888' }}>há 3 horas · Não lida</div></div><button className="ib">Marcar lida</button></div>
          <div className="nr"><div className="nd"></div><div><div style={{ fontSize: 11.5 }}>Bem-vindo ao GymnArte!</div><div style={{ fontSize: 10, color: '#888' }}>{u.cr} · Lida</div></div></div>
        </div>
      )}
    </div>
  )
}

// ── Login ─────────────────────────────────────────────────────────────────────
function LoginPage({ onLogin }: { onLogin: (a: Auth) => void }) {
  const [email, setEmail] = useState('')
  const [pass, setPass] = useState('')
  const [err, setErr] = useState('')
  const [loading, setLoading] = useState(false)

  async function submit(e: React.FormEvent) {
    e.preventDefault(); setErr(''); setLoading(true)
    try {
      const res = await fetch('http://localhost:5297/api/auth/login', {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password: pass })
      })
      if (!res.ok) throw new Error('Credenciais inválidas')
      const data = await res.json()
      const auth = { token: data.token, userId: data.userId, fullName: data.fullName, role: data.role }
      localStorage.setItem('ga_auth', JSON.stringify(auth))
      onLogin(auth)
    } catch {
      // Demo mode — sem API
      if (email === 'admin@gymnarte.pt' && pass === 'admin') {
        const auth = { token: 'demo', userId: 1003, fullName: 'João Rosa', role: 'Admin' }
        localStorage.setItem('ga_auth', JSON.stringify(auth))
        onLogin(auth)
      } else {
        setErr('Credenciais inválidas. Demo: admin@gymnarte.pt / admin')
      }
    }
    setLoading(false)
  }

  return (
    <div className="login-wrap">
      <div className="login-box">
        <div className="login-logo">GymnArte</div>
        <div className="login-sub">Backoffice · Área de administração</div>
        {err && <div className="lerr">{err}</div>}
        <form className="lf" onSubmit={submit}>
          <label>Email</label>
          <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="admin@gymnarte.pt" required autoFocus />
          <label>Password</label>
          <input type="password" value={pass} onChange={e => setPass(e.target.value)} placeholder="••••••••" required />
          <button className="lbtn" disabled={loading}>{loading ? 'A entrar...' : 'Entrar'}</button>
        </form>
      </div>
    </div>
  )
}

// ── Users Page ────────────────────────────────────────────────────────────────
function UsersPage({ auth }: { auth: Auth }) {
  const [users, setUsers] = useState<User[]>(MOCK_USERS)
  const [filt, setFilt] = useState<User[]>(MOCK_USERS)
  const [sel, setSel] = useState<User | null>(null)
  const [q, setQ] = useState('')
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState({ name: '', email: '', un: '', ph: '', scope: 'Member', g: 'Masculino', b: '' })

  function filter(v: string) {
    setQ(v)
    const lo = v.toLowerCase()
    setFilt(users.filter(u => u.name.toLowerCase().includes(lo) || String(u.id).includes(lo)))
  }

  function saveUser() {
    const ini = initials(form.name)
    const newU: User = { id: 1000 + users.length + 1, i: ini, c: avatarColor(form.name), name: form.name, un: form.un, email: form.email, ph: form.ph, g: form.g, b: form.b, scope: form.scope, cr: new Date().toLocaleDateString('pt-PT'), plans: 0, bio: null }
    const next = [...users, newU]
    setUsers(next); setFilt(next); setShowModal(false)
    setForm({ name: '', email: '', un: '', ph: '', scope: 'Member', g: 'Masculino', b: '' })
  }

  return (
    <>
      <div className="ul-wrap">
        <div className="ul-panel">
          <div className="ul-head">
            <span className="ul-ht">Sócios</span>
            <button className="btn btp" style={{ padding: '3px 9px', fontSize: 10.5 }} onClick={() => setShowModal(true)}>+ Novo</button>
          </div>
          <div className="ul-sr">
            <input value={q} onChange={e => filter(e.target.value)} placeholder="Nome ou nº de sócio..." />
          </div>
          <div className="ul-list">
            {filt.map(u => (
              <div key={u.id} className={`ur${sel?.id === u.id ? ' sel' : ''}`} onClick={() => setSel(u)}>
                <div className="uav" style={{ background: u.c }}>{u.i}</div>
                <div><div className="ur-n">{u.name}</div><div className="ur-m">#{u.id} · {u.scope}</div></div>
              </div>
            ))}
          </div>
        </div>
        {sel ? <UserDetail u={sel} onClose={() => setSel(null)} /> : (
          <div className="ud"><div className="es">
            <div className="es-ic"><svg width="20" height="20" viewBox="0 0 20 20" fill="none"><circle cx="10" cy="6.5" r="3.5" stroke="#aaa" strokeWidth="1.5"/><path d="M3 18c0-3.9 3.1-7 7-7s7 3.1 7 7" stroke="#aaa" strokeWidth="1.5" strokeLinecap="round"/></svg></div>
            <p style={{ fontSize: 12.5, color: '#999' }}>Seleciona um sócio</p>
            <p style={{ fontSize: 11, color: '#bbb' }}>{users.length} sócios registados</p>
          </div></div>
        )}
      </div>
      {showModal && (
        <Modal title="Novo sócio" onClose={() => setShowModal(false)}>
          <div className="mf">
            <div className="mrow">
              <div><label>Nome completo</label><input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} placeholder="João Silva" /></div>
              <div><label>Username</label><input value={form.un} onChange={e => setForm({ ...form, un: e.target.value })} placeholder="joao.silva" /></div>
            </div>
            <label>Email</label><input value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} placeholder="joao@email.com" type="email" />
            <div className="mrow">
              <div><label>Telefone</label><input value={form.ph} onChange={e => setForm({ ...form, ph: e.target.value })} placeholder="9XX XXX XXX" /></div>
              <div><label>Data nascimento</label><input type="date" value={form.b} onChange={e => setForm({ ...form, b: e.target.value })} /></div>
            </div>
            <div className="mrow">
              <div><label>Género</label><select value={form.g} onChange={e => setForm({ ...form, g: e.target.value })}><option>Masculino</option><option>Feminino</option><option>Outro</option></select></div>
              <div><label>Perfil</label><select value={form.scope} onChange={e => setForm({ ...form, scope: e.target.value })}><option>Member</option><option>Trainer</option><option>Admin</option></select></div>
            </div>
          </div>
          <div className="mfooter">
            <button className="btn" onClick={() => setShowModal(false)}>Cancelar</button>
            <button className="btn btp" onClick={saveUser}>Criar sócio</button>
          </div>
        </Modal>
      )}
    </>
  )
}

// ── Exercises Page ────────────────────────────────────────────────────────────
function ExercisesPage() {
  const [exs, setExs] = useState<Exercise[]>(MOCK_EX)
  const [filt, setFilt] = useState<Exercise[]>(MOCK_EX)
  const [showModal, setShowModal] = useState(false)
  const [editEx, setEditEx] = useState<Exercise | null>(null)
  const [form, setForm] = useState({ name: '', type: 'Musculação', typeId: 1, notes: '' })

  function filter(q: string, type: string) {
    setFilt(exs.filter(e => (e.name.toLowerCase().includes(q.toLowerCase())) && (type === '' || e.type === type)))
  }

  function openNew() { setEditEx(null); setForm({ name: '', type: 'Musculação', typeId: 1, notes: '' }); setShowModal(true) }
  function openEdit(e: Exercise) { setEditEx(e); setForm({ name: e.name, type: e.type, typeId: e.typeId, notes: e.notes }); setShowModal(true) }

  function save() {
    if (editEx) {
      const next = exs.map(e => e.id === editEx.id ? { ...e, ...form } : e)
      setExs(next); setFilt(next)
    } else {
      const next = [...exs, { id: exs.length + 1, ...form, inPlans: 0 }]
      setExs(next); setFilt(next)
    }
    setShowModal(false)
  }

  function del(id: number) {
    const next = exs.filter(e => e.id !== id); setExs(next); setFilt(next)
  }

  return (
    <>
      <div className="sr">
        <input placeholder="Pesquisar exercício..." onChange={e => filter(e.target.value, '')} />
        <select onChange={e => filter('', e.target.value)}><option value="">Todos os tipos</option><option>Musculação</option><option>Cardio</option><option>Flexibilidade</option><option>Misto</option></select>
        <button className="btn btp" onClick={openNew}>+ Novo exercício</button>
      </div>
      <div className="pi">
        <div className="card">
          <div className="ch"><span className="ct">Exercícios</span><span style={{ fontSize: 10, color: '#888' }}>{filt.length} registos</span></div>
          <table><thead><tr><th>Nome</th><th>Tipo</th><th>Notas</th><th>Em planos</th><th></th></tr></thead>
            <tbody>{filt.map(e => (
              <tr key={e.id}>
                <td>{e.name}</td>
                <td>{typeBadge(e.type)}</td>
                <td style={{ color: '#888' }}>{e.notes || '—'}</td>
                <td>{e.inPlans}</td>
                <td><div style={{ display: 'flex', gap: 4 }}>
                  <button className="ib" onClick={() => openEdit(e)}>Editar</button>
                  <button className="ib dl" onClick={() => del(e.id)}>Remover</button>
                </div></td>
              </tr>
            ))}</tbody>
          </table>
        </div>
      </div>
      {showModal && (
        <Modal title={editEx ? 'Editar exercício' : 'Novo exercício'} onClose={() => setShowModal(false)}>
          <div className="mf">
            <label>Nome</label><input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} placeholder="Nome do exercício" />
            <label>Tipo</label>
            <select value={form.type} onChange={e => setForm({ ...form, type: e.target.value })}>
              <option>Musculação</option><option>Cardio</option><option>Flexibilidade</option><option>Misto</option>
            </select>
            <label>Notas</label><textarea value={form.notes} onChange={e => setForm({ ...form, notes: e.target.value })} placeholder="Observações..." />
          </div>
          <div className="mfooter">
            <button className="btn" onClick={() => setShowModal(false)}>Cancelar</button>
            <button className="btn btp" onClick={save}>{editEx ? 'Guardar' : 'Criar'}</button>
          </div>
        </Modal>
      )}
    </>
  )
}

// ── ExerciseTypes Page ────────────────────────────────────────────────────────
function ExTypesPage() {
  const [types, setTypes] = useState<ExType[]>(MOCK_TYPES)
  const [showModal, setShowModal] = useState(false)
  const [editT, setEditT] = useState<ExType | null>(null)
  const [form, setForm] = useState({ name: '', desc: '' })

  function openNew() { setEditT(null); setForm({ name: '', desc: '' }); setShowModal(true) }
  function openEdit(t: ExType) { setEditT(t); setForm({ name: t.name, desc: t.desc }); setShowModal(true) }
  function save() {
    if (editT) setTypes(types.map(t => t.id === editT.id ? { ...t, ...form } : t))
    else setTypes([...types, { id: types.length + 1, ...form, count: 0, created: new Date().toLocaleDateString('pt-PT'), updated: '—', updatedBy: '—' }])
    setShowModal(false)
  }

  return (
    <>
      <div className="sr">
        <input placeholder="Pesquisar tipo..." />
        <button className="btn btp" onClick={openNew}>+ Novo tipo</button>
      </div>
      <div className="pi">
        <div className="card">
          <div className="ch"><span className="ct">Tipos de exercício</span></div>
          <table><thead><tr><th>ID</th><th>Nome</th><th>Descrição</th><th>Exercícios</th><th>Criado em</th><th>Atualizado</th><th>Por</th><th></th></tr></thead>
            <tbody>{types.map(t => (
              <tr key={t.id}>
                <td style={{ color: '#888' }}>#{t.id}</td>
                <td><strong>{t.name}</strong></td>
                <td style={{ color: '#666' }}>{t.desc}</td>
                <td>{t.count}</td>
                <td style={{ color: '#888' }}>{t.created}</td>
                <td style={{ color: '#888' }}>{t.updated}</td>
                <td style={{ color: '#888' }}>{t.updatedBy}</td>
                <td><div style={{ display: 'flex', gap: 4 }}>
                  <button className="ib" onClick={() => openEdit(t)}>Editar</button>
                  <button className="ib dl">Remover</button>
                </div></td>
              </tr>
            ))}</tbody>
          </table>
        </div>
      </div>
      {showModal && (
        <Modal title={editT ? 'Editar tipo' : 'Novo tipo'} onClose={() => setShowModal(false)}>
          <div className="mf">
            <label>Nome</label><input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} placeholder="Ex: Musculação" />
            <label>Descrição</label><textarea value={form.desc} onChange={e => setForm({ ...form, desc: e.target.value })} placeholder="Descrição do tipo de exercício..." />
          </div>
          <div className="mfooter">
            <button className="btn" onClick={() => setShowModal(false)}>Cancelar</button>
            <button className="btn btp" onClick={save}>{editT ? 'Guardar' : 'Criar'}</button>
          </div>
        </Modal>
      )}
    </>
  )
}

// ── Scopes Page ───────────────────────────────────────────────────────────────
function ScopesPage() {
  const [scopes] = useState<Scope[]>(MOCK_SCOPES)
  return (
    <>
      <div className="sr"><button className="btn btp">+ Novo perfil</button></div>
      <div className="pi">
        <div className="card">
          <div className="ch"><span className="ct">Perfis de acesso</span></div>
          <table><thead><tr><th>ID</th><th>Role</th><th>Utilizadores</th><th>Criado em</th><th>Última atualização</th><th>Atualizado por</th><th></th></tr></thead>
            <tbody>{scopes.map(s => {
              const cls = s.role === 'Admin' ? 'gbr' : s.role === 'Trainer' ? 'gba' : 'gbb'
              return (
                <tr key={s.id}>
                  <td style={{ color: '#888' }}>#{s.id}</td>
                  <td><span className={`badge ${cls}`}>{s.role}</span></td>
                  <td>{s.users}</td>
                  <td style={{ color: '#888' }}>{s.created}</td>
                  <td style={{ color: '#888' }}>{s.updated}</td>
                  <td style={{ color: '#888' }}>{s.updatedBy}</td>
                  <td><button className="ib">Editar</button></td>
                </tr>
              )
            })}</tbody>
          </table>
        </div>
      </div>
    </>
  )
}

// ── Shell ─────────────────────────────────────────────────────────────────────
function Shell({ auth, onLogout }: { auth: Auth; onLogout: () => void }) {
  const [page, setPage] = useState('users')
  const ini = initials(auth.fullName)

  const nav = [
    { id: 'users', label: 'Utilizadores', count: 148, icon: <svg viewBox="0 0 16 16" fill="currentColor"><circle cx="8" cy="5" r="3"/><path d="M2 14c0-3.3 2.7-6 6-6s6 2.7 6 6"/></svg> },
    { id: 'exercises', label: 'Exercícios', count: 94, icon: <svg viewBox="0 0 16 16" fill="currentColor"><circle cx="3" cy="8" r="2"/><circle cx="13" cy="8" r="2"/><rect x="4.5" y="6.5" width="7" height="3" rx="1.5"/></svg> },
    { id: 'etypes', label: 'Tipos de exercício', count: 4, icon: <svg viewBox="0 0 16 16" fill="currentColor"><rect x="1" y="3" width="14" height="2" rx="1"/><rect x="1" y="7" width="10" height="2" rx="1"/><rect x="1" y="11" width="7" height="2" rx="1"/></svg> },
    { id: 'scopes', label: 'Perfis de acesso', count: 3, icon: <svg viewBox="0 0 16 16" fill="currentColor"><rect x="2" y="6" width="12" height="8" rx="1"/><path d="M5 6V4a3 3 0 016 0v2" stroke="currentColor" strokeWidth="1.5" fill="none"/></svg> },
  ]
  const labels: Record<string, string> = { users: 'Utilizadores', exercises: 'Exercícios', etypes: 'Tipos de exercício', scopes: 'Perfis de acesso' }

  return (
    <div className="shell">
      <div className="sb">
        <div className="sb-top">
          <div className="sb-brand">GymnArte</div>
          <div className="sb-sub">Backoffice Admin</div>
        </div>
        <div className="sb-sec">Gestão</div>
        {nav.map(n => (
          <div key={n.id} className={`ni${page === n.id ? ' on' : ''}`} onClick={() => setPage(n.id)}>
            {n.icon}{n.label}<span className="ni-ct">{n.count}</span>
          </div>
        ))}
        <div className="sb-bot">
          <div className="ni" onClick={onLogout}>
            <svg viewBox="0 0 16 16" fill="currentColor"><path d="M6 2H3a1 1 0 00-1 1v10a1 1 0 001 1h3M10 11l3-3-3-3M13 8H6" stroke="currentColor" strokeWidth="1.3" fill="none" strokeLinecap="round"/></svg>
            Terminar sessão
          </div>
        </div>
      </div>
      <div className="main">
        <div className="topbar">
          <div className="bc">{labels[page]}</div>
          <div className="tpr">
            <span style={{ fontSize: 11, color: '#888' }}>{auth.fullName} · {auth.role}</span>
            <div className="tav">{ini}</div>
          </div>
        </div>
        <div className="body">
          <div className={`pg${page === 'users' ? ' on' : ''}`} id="pg-users"><UsersPage auth={auth} /></div>
          <div className={`pg${page === 'exercises' ? ' on' : ''}`} id="pg-exercises"><ExercisesPage /></div>
          <div className={`pg${page === 'etypes' ? ' on' : ''}`} id="pg-etypes"><ExTypesPage /></div>
          <div className={`pg${page === 'scopes' ? ' on' : ''}`} id="pg-scopes"><ScopesPage /></div>
        </div>
      </div>
    </div>
  )
}

// ── App ───────────────────────────────────────────────────────────────────────
export default function App() {
  const [auth, setAuth] = useState<Auth | null>(() => {
    try { const s = localStorage.getItem('ga_auth'); return s ? JSON.parse(s) : null } catch { return null }
  })
  function logout() { localStorage.removeItem('ga_auth'); setAuth(null) }
  if (!auth) return <LoginPage onLogin={setAuth} />
  return <Shell auth={auth} onLogout={logout} />
}
